using System;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO.Inport;
using CarDealer.Models;
using CarDealer.XMLHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CarDealer.DTO.Output;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //var xmlSuppliers = File.ReadAllText("./Datasets/suppliers.xml");
            //var xmlParts = File.ReadAllText("./Datasets/parts.xml");
            //var xmlCars = File.ReadAllText("./Datasets/cars.xml");
            //var xmlCustomers = File.ReadAllText("./Datasets/customers.xml");
            //var xmlSales = File.ReadAllText("./Datasets/sales.xml");

            //Console.WriteLine(ImportSuppliers(context, xmlSuppliers));
            //Console.WriteLine(ImportParts(context, xmlParts));
            //Console.WriteLine(ImportCars(context, xmlCars));
            //Console.WriteLine(ImportCustomers(context, xmlCustomers));
            //Console.WriteLine(ImportSales(context, xmlSales));

            //Console.WriteLine(GetCarsWithDistance(context));
            //Console.WriteLine(GetCarsFromMakeBmw(context));
            //Console.WriteLine(GetLocalSuppliers(context));
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));
            //Console.WriteLine(GetTotalSalesByCustomer(context));
            //Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new SalesOutputModel
                {
                    Car = new CarOutputModel
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.Sales.SelectMany(x => x.Car.PartCars).Sum(y => y.Part.Price),
                    PriceWithDiscount = s.Car.Sales.SelectMany(x => x.Car.PartCars).Sum(y => y.Part.Price) * (100 - s.Discount) / 100
                })
                .ToArray();

            return XMLConverter.Serialize(sales, "sales");
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count >= 1)
                .Select(c => new CustomerOutputModel
                {
                    FullName = c.Name,
                    CarsBought = c.Sales.Count,
                    SpentMoney = c.Sales
                        .Select(ca => ca.Car)
                        .SelectMany(pc => pc.PartCars)
                        .Sum(p => p.Part.Price)
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            return XMLConverter.Serialize(customers, "customers");
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new CarsWithPartsOutputModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.PartCars.Select(pc => new PartOutputModel
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price
                    })
                        .OrderByDescending(pc => pc.Price)
                        .ToArray()
                })
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            return XMLConverter.Serialize(cars, "cars");
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new LocalSuppliersOutputModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            return XMLConverter.Serialize(suppliers, "suppliers");
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(c => new BMWCarsOutputModel
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            //InitializeMapper();
            //var carsDTO = mapper.Map<BMWCarsOutputModel[]>(cars);

            return XMLConverter.Serialize(cars, "cars");
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TravelledDistance > 2_000_000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToList();

            InitializeMapper();
            var carsDTO = mapper.Map<ICollection<CarsWithDistanceOutputModel>>(cars).ToArray();

            return XMLConverter.Serialize(carsDTO, "cars");
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var carsId = context.Cars.Select(c => c.Id).ToArray();

            var salesDTO = XMLConverter.Deserializer<SaleInputModel>(inputXml, "Sales")
                .Where(s => carsId.Contains(s.CarId));

            InitializeMapper();
            var sales = mapper.Map<ICollection<Sale>>(salesDTO);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var customersDTO = XMLConverter.Deserializer<CustomerInputModel>(inputXml, "Customers");

            InitializeMapper();
            var customers = mapper.Map<ICollection<Customer>>(customersDTO);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var partsId = context.Parts.Select(p => p.Id).ToList();

            var carsDTO = XMLConverter.Deserializer<CarInputModel>(inputXml, "Cars");

            var cars = carsDTO
                .Select(c => new Car
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TraveledDistance,
                    PartCars = c.Parts
                        .Select(p => p.PartId)
                        .Distinct()
                        .Intersect(partsId)
                        .Select(pc => new PartCar
                        {
                            PartId = pc
                        })
                        .ToList()

                })
                .ToList();

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var suppliers = context.Suppliers.Select(s => s.Id);

            var partsDTO = XMLConverter
                .Deserializer<PartInputModel>(inputXml, "Parts")
                .Where(p => suppliers.Contains(p.SupplierId));

            InitializeMapper();

            var parts = mapper.Map<ICollection<Part>>(partsDTO);

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var suppliersDTO = XMLConverter
                .Deserializer<SupplierInputModel>(inputXml, "Suppliers");

            InitializeMapper();
            var suppliers = mapper.Map<IEnumerable<Supplier>>(suppliersDTO);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}";
        }

        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            mapper = config.CreateMapper();
        }
    }
}