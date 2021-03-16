using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            string suppliersJson = File.ReadAllText("suppliers.json");
            string partsJson = File.ReadAllText("parts.json");
            string carsJson = File.ReadAllText("cars.json");
            string customersJson = File.ReadAllText("customers.json");
            string salesJson = File.ReadAllText("sales.json");

            Console.WriteLine(ImportSuppliers(context, suppliersJson));
            Console.WriteLine(ImportParts(context, partsJson));
            Console.WriteLine(ImportCars(context, carsJson));
            Console.WriteLine(ImportCustomers(context, customersJson));
            Console.WriteLine(ImportSales(context, salesJson));
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var salesDTO = JsonConvert.DeserializeObject<IEnumerable<SaleDTO>>(inputJson);

            InitializeMapper();

            var sales = mapper.Map<IEnumerable<Sale>>(salesDTO);
            //TODO: verify carId and customerID

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customersDTO = JsonConvert.DeserializeObject<IEnumerable<CustomerDTO>>(inputJson);

            InitializeMapper();
            var customers = mapper.Map<IEnumerable<Customer>>(customersDTO);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDTO = JsonConvert.DeserializeObject<IEnumerable<CarDTO>>(inputJson);

            InitializeMapper();
            var cars = new List<Car>();

            foreach (var car in carsDTO)
            {
                var currentCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance
                };

                foreach (var part in car.PartsId)
                {
                    var partCar = new PartCar
                    {
                        PartId = part
                    };

                    currentCar.PartCars.Add(partCar);
                }

                cars.Add(currentCar);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count()}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var partsDTO = JsonConvert.DeserializeObject<IEnumerable<PartDTO>>(inputJson);

            InitializeMapper();
            var suppliers = context.Suppliers.Select(s => s.Id);
            var parts = mapper.Map<IEnumerable<Part>>(partsDTO)
                .Where(p => suppliers.Contains(p.SupplierId));

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}.";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliersDTO = JsonConvert.DeserializeObject<IEnumerable<SupplierDTO>>(inputJson);

            InitializeMapper();
            var suppliers = mapper.Map<IEnumerable<Supplier>>(suppliersDTO);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}.";
        }

        public static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            mapper = config.CreateMapper();
        }
    }
}