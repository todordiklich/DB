namespace VaporStore.DataProcessor
{
	using System;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{

            var games = context.Genres.ToList()
                .Where(g => genreNames.Contains(g.Name) && g.Games.Any(gm => gm.Purchases.Count > 0))
                .Select(g => new
                {
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games.Where(gm => gm.Purchases.Count > 0).Select(game => new
                    {
                        Id = game.Id,
                        Title = game.Name,
                        Developer = game.Developer.Name,
                        Tags = String.Join(", ", game.GameTags.Select(t => t.Tag.Name)),
                        Players = game.Purchases.Count
                    })
                            .OrderByDescending(x => x.Players)
                            .ThenBy(x => x.Id),
                    TotalPlayers = g.Games.Select(gm => gm.Purchases.Count()).ToList().Sum(),
                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(x => x.Id);

            return JsonConvert.SerializeObject(games , Formatting.Indented);
        }

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
            var userPurchases = context.Users.ToList()
                .Where(u => u.Cards.Any(c => c.Purchases.Any(p => p.Type.ToString() == storeType)))
                .Select(u => new UserExportViewModel
                {
                    Username = u.Username,
                    Purchases = u.Cards.SelectMany(c => c.Purchases)
                        .Where(p => p.Type.ToString() == storeType)
                        .Select(p => new PurchaseExportViewModel
                        {
                            CardNumber = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new GameExportViewModel
                            {
                                Title = p.Game.Name,
                                Genre = p.Game.Genre.Name,
                                Price = p.Game.Price
                            }
                        })
                            .OrderBy(p => p.Date)
                            .ToArray(),
                    TotalSpent = u.Cards
                    .SelectMany(c => c.Purchases.Where(p => p.Type.ToString() == storeType))
                    .Sum(p => p.Game.Price)
                })
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

			return XmlConverter.Serialize(userPurchases, "Users");
		}
	}
}