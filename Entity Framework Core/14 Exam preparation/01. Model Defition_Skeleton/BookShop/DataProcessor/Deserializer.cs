namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var bookImportViewModels = XmlConverter.Deserializer<BookImportViewModel>(xmlString, "Books");

            foreach (var bookImportViewModel in bookImportViewModels)
            {
                if (!IsValid(bookImportViewModel))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var book = new Book
                {
                    Name = bookImportViewModel.Name,
                    Genre = (Genre)bookImportViewModel.Genre,
                    Price = bookImportViewModel.Price,
                    Pages = bookImportViewModel.Pages,
                    PublishedOn = DateTime.ParseExact(bookImportViewModel.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                };

                context.Add(book);
                sb.AppendLine(String.Format(SuccessfullyImportedBook, book.Name, book.Price));
            }

            context.SaveChanges();


            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var authorImportViewModels = JsonConvert.DeserializeObject<ICollection<AuthorsImportViewModel>>(jsonString);

            var allbooksIds = context.Books.Select(b => b.Id).ToList();

            foreach (var authorImportViewModel in authorImportViewModels)
            {
                var authorBooks = authorImportViewModel.Books.Where(b => b.Id != null);
                var booksToAdd = new List<int>();

                foreach (var authorBook in authorBooks)
                {
                    if (allbooksIds.Contains(authorBook.Id.Value))
                    {
                        booksToAdd.Add(authorBook.Id.Value);
                    }
                }

                if (!IsValid(authorImportViewModel) ||
                    context.Authors.Any(a => a.Email == authorImportViewModel.Email) ||
                    booksToAdd.Count() == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var author = new Author
                {
                    FirstName = authorImportViewModel.FirstName,
                    LastName = authorImportViewModel.LastName,
                    Phone = authorImportViewModel.Phone,
                    Email = authorImportViewModel.Email,
                    AuthorsBooks = booksToAdd
                        .Select(b => new AuthorBook 
                            {
                                BookId = b
                            })
                            .ToList()
                };

                context.Authors.Add(author);
                context.SaveChanges();
                sb.AppendLine(String.Format(SuccessfullyImportedAuthor, author.FirstName + " " + author.LastName, author.AuthorsBooks.Count));
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}