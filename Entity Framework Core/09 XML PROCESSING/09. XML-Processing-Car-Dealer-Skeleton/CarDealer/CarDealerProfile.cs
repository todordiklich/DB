using AutoMapper;

using CarDealer.Models;
using CarDealer.DTO.Inport;
using CarDealer.DTO.Output;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<SupplierInputModel, Supplier>();
            CreateMap<PartInputModel, Part>();
            CreateMap<CustomerInputModel, Customer>();
            CreateMap<SaleInputModel, Sale>();

            CreateMap<Car, CarsWithDistanceOutputModel>();
            CreateMap<Car, BMWCarsOutputModel>();
        }
    }
}
