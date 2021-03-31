namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var gamesImportViewModels = JsonConvert
                .DeserializeObject<ICollection<GameImportViewModel>>(jsonString);

            foreach (var gameImportViewModel in gamesImportViewModels)
            {
                if (!IsValid(gameImportViewModel) || gameImportViewModel.Tags.Count() == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var developer = context.Developers
                    .Where(d => d.Name == gameImportViewModel.Developer).FirstOrDefault()
                    ?? new Developer { Name = gameImportViewModel.Developer };

                var genre = context.Genres
                    .Where(g => g.Name == gameImportViewModel.Genre).FirstOrDefault()
                    ?? new Genre { Name = gameImportViewModel.Genre };

                var game = new Game()
                {
                    Name = gameImportViewModel.Name,
                    Price = gameImportViewModel.Price,
                    ReleaseDate = gameImportViewModel.ReleaseDate.Value,
                    Developer = developer,
                    Genre = genre,
                };

                foreach (var tagImportViewModel in gameImportViewModel.Tags)
                {
                    var tag = context.Tags
                        .Where(t => t.Name == tagImportViewModel).FirstOrDefault()
                        ?? new Tag { Name = tagImportViewModel };
                    game.GameTags.Add(new GameTag { Tag = tag });
                }

                context.Games.Add(game);
                context.SaveChanges();

                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count()} tags");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var userImputViewModels = JsonConvert
                .DeserializeObject<ICollection<UserImportViewModel>>(jsonString);

            foreach (var userImportViewModel in userImputViewModels)
            {
                if (!IsValid(userImportViewModel) || !userImportViewModel.Cards.Any(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var user = new User
                {
                    FullName = userImportViewModel.FullName,
                    Username = userImportViewModel.Username,
                    Email = userImportViewModel.Email,
                    Age = userImportViewModel.Age,
                    Cards = userImportViewModel.Cards.Select(c => new Card
                    {
                        Number = c.Number,
                        Cvc = c.CVC,
                        Type = c.Type.Value
                    })
                    .ToList()
                };

                context.Users.Add(user);
                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
            }

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var purchasesImportViewModels = XmlConverter.Deserializer<PurchasesImportViewModel>(xmlString, "Purchases");

            foreach (var purchasesImportViewModel in purchasesImportViewModels)
            {
                if (!IsValid(purchasesImportViewModel))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var isDateParsed = DateTime.TryParseExact(purchasesImportViewModel.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

                var purchase = new Purchase
                {
                    Game = context.Games
                        .FirstOrDefault(g => g.Name == purchasesImportViewModel.GameName),
                    Type = purchasesImportViewModel.Type.Value,
                    ProductKey = purchasesImportViewModel.ProductKey,
                    Card = context.Cards
                        .FirstOrDefault(c => c.Number == purchasesImportViewModel.CardNumber),
                    Date = date
                };

                context.Purchases.Add(purchase);
                sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
            }

            context.SaveChanges();

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