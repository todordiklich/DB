namespace VaporStore.DataProcessor
{
	using System;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;

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


			return "TODO";
		}
	}
}