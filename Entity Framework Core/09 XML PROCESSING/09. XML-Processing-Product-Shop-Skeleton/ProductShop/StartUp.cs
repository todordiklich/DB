using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            string usersXML = File.ReadAllText("./Datasets/users.xml");
            string productsXML = File.ReadAllText("./Datasets/products.xml");

            System.Console.WriteLine(ImportUsers(context, usersXML));
            System.Console.WriteLine(ImportProducts(context, productsXML));
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var root = new XmlRootAttribute("Products");
            var xmlSerializer = new XmlSerializer(typeof(ProductInputModel[]), root);

            var textReader = new StringReader(inputXml);

            var productsInputModel = xmlSerializer.Deserialize(textReader) as IEnumerable<ProductInputModel>;

            InitializeMapper();
            var products = mapper.Map<IEnumerable<Product>>(productsInputModel);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var root = new XmlRootAttribute("Users");
            var xmlSerializer = new XmlSerializer(typeof(UserInputModel[]), root);

            var textReader = new StringReader(inputXml);

            var usersInputModel = xmlSerializer.Deserialize(textReader) as IEnumerable<UserInputModel>;

            InitializeMapper();
            var users = mapper.Map<IEnumerable<User>>(usersInputModel);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            mapper = config.CreateMapper();
        }
    }
}