using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //string usersXML = File.ReadAllText("./Datasets/users.xml");
            //string productsXML = File.ReadAllText("./Datasets/products.xml");
            //string categoriesXML = File.ReadAllText("./Datasets/categories.xml");
            //string categoryProductsXML = File.ReadAllText("./Datasets/categories-products.xml");

            //Console.WriteLine(ImportUsers(context, usersXML));
            //Console.WriteLine(ImportProducts(context, productsXML));
            //Console.WriteLine(ImportCategories(context, categoriesXML));
            //Console.WriteLine(ImportCategoryProducts(context, categoryProductsXML));

            //Console.WriteLine(GetProductsInRange(context));
            //Console.WriteLine(GetSoldProducts(context));
            //Console.WriteLine(GetCategoriesByProductsCount(context));
            //Console.WriteLine(GetUsersWithProducts(context));
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var root = new XmlRootAttribute("Users");
            var serializer = new XmlSerializer(typeof(UsersAndProductsOutputModel), root);

            var users = new UsersAndProductsOutputModel
            {
                Count = context.Users
                .Include(u => u.ProductsSold)
                .ToArray()
                        .Where(us => us.ProductsSold.Any(p => p.BuyerId != null)).Count(),

                Users = context.Users
                .Include(u => u.ProductsSold)
                .ToArray()
                   .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                   .Select(user => new UserOutputModel
                   {
                       FirstName = user.FirstName,
                       LastName = user.LastName,
                       Age = user.Age,
                       SoldProducts = user.ProductsSold
                           .Where(p => p.BuyerId != null)
                           .Select(p => new SoldProductsOutputModel
                           {
                               Count = user.ProductsSold.Where(pr => pr.BuyerId != null).Count(),
                               Products = user.ProductsSold
                                   .Where(pr => pr.BuyerId != null)
                                   .Select(product => new ProductOutputModel
                                   {
                                       Name = product.Name,
                                       Price = product.Price,
                                   })
                                   .OrderByDescending(prd => prd.Price)
                                   .ToArray()
                           })
                           .FirstOrDefault()
                   })
                   .OrderByDescending(u => u.SoldProducts.Count)
                   .Take(10)
                   .ToArray()
            };

            var builder = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            using (var writer = new StringWriter(builder))
            {
                serializer.Serialize(writer, users, namespaces);
            }

            return builder.ToString().TrimEnd();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var root = new XmlRootAttribute("Categories");
            var serializer = new XmlSerializer(typeof(CategoryOutputModel[]), root);
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            var categories = context.Categories
                .Select(c => new CategoryOutputModel
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(p => p.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            var builder = new StringBuilder();

            using (var writer = new StringWriter(builder))
            {
                serializer.Serialize(writer, categories, namespaces);
            }

            return builder.ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var root = new XmlRootAttribute("Users");
            var ser = new XmlSerializer(typeof(UsersWithSoldProductsOutputModel[]), root);
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new UsersWithSoldProductsOutputModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                        .Where(ps => ps.BuyerId != null)
                        .Select(sp => new SoldProduct
                        {
                            Name = sp.Name,
                            Price = sp.Price,
                        })
                        .ToArray()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ToArray();

            var builder = new StringBuilder();

            using (var writer = new StringWriter(builder))
            {
                ser.Serialize(writer, users, namespaces);
            }

            return builder.ToString().TrimEnd();
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var root = new XmlRootAttribute("Products");
            var ser = new XmlSerializer(typeof(ProductsInRangeOutputModel[]), root);
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            var productsOutputModel = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ProductsInRangeOutputModel
                {
                    Name = p.Name,
                    Price = p.Price,
                    BuyerFullName = p.Buyer.FirstName + " " + p.Buyer.LastName,
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ToArray();

            var builder = new StringBuilder();

            using (var writer = new StringWriter(builder))
            {
                ser.Serialize(writer, productsOutputModel, namespaces);
            }

            return builder.ToString().TrimEnd();
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var root = new XmlRootAttribute("CategoryProducts");
            var xmlSerializer = new XmlSerializer(typeof(CategoryProductInputModel[]), root);
            var textReader = new StringReader(inputXml);

            var categoryProductsInputModels = xmlSerializer.Deserialize(textReader) as IEnumerable<CategoryProductInputModel>;

            var categoriesId = context.Categories.Select(c => c.Id);
            var productsId = context.Products.Select(p => p.Id);

            InitializeMapper();
            var categoryProducts = mapper
                .Map<IEnumerable<CategoryProduct>>(categoryProductsInputModels)
                .Where(cp => categoriesId.Contains(cp.CategoryId)
                          && productsId.Contains(cp.ProductId));

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var root = new XmlRootAttribute("Categories");
            var xmlSerializer = new XmlSerializer(typeof(CategoryInputModel[]), root);
            var textReader = new StringReader(inputXml);

            var categoriesInputModels = (xmlSerializer.Deserialize(textReader) as IEnumerable<CategoryInputModel>)?.Where(c => c.Name != null);

            InitializeMapper();
            var categories = mapper.Map<IEnumerable<Category>>(categoriesInputModels);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
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