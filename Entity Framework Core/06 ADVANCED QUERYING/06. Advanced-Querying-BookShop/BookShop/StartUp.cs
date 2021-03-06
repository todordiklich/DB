namespace BookShop
{
    using Data;
    using System;
    using Initializer;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using BookShop.Models.Enums;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //Test solution below:
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.Books.RemoveRange(books);
            context.SaveChanges();

            return books.Count;
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Books = c.CategoryBooks
                             .Select(b => new
                             {
                                 BookTitle = b.Book.Title,
                                 ReleaseDate = b.Book.ReleaseDate.Value
                             })
                             .OrderByDescending(b => b.ReleaseDate)
                             .Take(3)
                             .ToList()
                })
                .OrderBy(c => c.CategoryName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.BookTitle} ({book.ReleaseDate.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var profits = context.Categories
                .Select(c => new
                {
                    Category = c.Name,
                    Profit = c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.Category)
                .ToList();

            return String.Join(Environment.NewLine, profits.Select(p => $"{p.Category} ${p.Profit:F2}"));
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                    BooksCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.BooksCopies)
                .ToList();

            return String.Join(Environment.NewLine, authors.Select(a => $"{a.FullName} - {a.BooksCopies}"));
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToList();

            return books.Count;
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => EF.Functions.Like(b.Author.LastName, $"{input}%"))
                .Select(b => new 
                {
                    Id = b.BookId,
                    Title = b.Title,
                    AuthorName = b.Author.FirstName + " " + b.Author.LastName
                })
                .OrderBy(b => b.Id)
                .ToList();

            return String.Join(Environment.NewLine, books.Select(b => $"{b.Title} ({b.AuthorName})"));
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => EF.Functions.Like(b.Title, $"%{input}%"))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => EF.Functions.Like(a.FirstName, $"%{input}"))
                .Select(a => new
                {
                    FullName =  a.FirstName + " " + a.LastName
                })
                .OrderBy(a => a.FullName)
                .ToList();

            return String.Join(Environment.NewLine, authors.Select(a => a.FullName));
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime comparisonDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate.Value < comparisonDate)
                .Select(b => new
                {
                    Title = b.Title,
                    Edition = b.EditionType,
                    Price = b.Price,
                    ReleaseDate = b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            return String.Join(Environment.NewLine, books.Select(b => $"{b.Title} - {b.Edition} - ${b.Price:F2}"));
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLower())
                .ToList();

            var books = context.BooksCategories
                .Where(bc => categories.Contains(bc.Category.Name.ToLower()))
                .Select(bc => bc.Book.Title)
                .OrderBy(bc => bc)
                .ToList();

            return String.Join(Environment.NewLine, books);
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new { b.Title, b.BookId })
                .OrderBy(b => b.BookId)
                .ToList();

            return String.Join(Environment.NewLine, books.Select(b => $"{b.Title}"));
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    Title = b.Title,
                    Price = b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToList();

            return String.Join(Environment.NewLine, books.Select(b => $"{b.Title} - ${b.Price:F2}"));
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5_000)
                .Select(b => new
                {
                    BookId = b.BookId,
                    Title = b.Title
                })
                .OrderBy(b => b.BookId)
                .ToList();

            return String.Join(Environment.NewLine, books.Select(b => b.Title));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction ageRestriction;
            var isParsed = Enum.TryParse<AgeRestriction>(command, true, out ageRestriction);

            if (isParsed)
            {
                var books = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(x => x)
                .ToList();

                return String.Join(Environment.NewLine, books);
            }
            else
            {
                return "Incorrect age restriction";
            }
        }
    }
}
