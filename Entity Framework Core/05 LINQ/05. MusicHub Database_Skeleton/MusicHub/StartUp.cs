namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            //DbInitializer.ResetDatabase(context);

            //Test your solutions here
            var result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var producerAlbums = context.Producers
                .ToList()
                .Where(x => x.Id == producerId)
                .Select(producer => new
                {
                    Albums = producer.Albums
                    .Select(album => new
                    {
                        AlbumName = album.Name,
                        ReleaseDate = album.ReleaseDate,
                        ProducerName = album.Producer.Name,
                        Songs = album.Songs
                            .Select(song => new
                            {
                                SongName = song.Name,
                                Price = song.Price,
                                Writer = song.Writer.Name
                            })
                            .OrderByDescending(x => x.SongName)
                            .ThenBy(x => x.Writer)
                            .ToList(),
                        AlbumPrice = album.Songs.Sum(s => s.Price)
                    })
                    .OrderByDescending(x => x.AlbumPrice)
                    .ToList()
                })
                .FirstOrDefault();

            var sb = new StringBuilder();

            foreach (var album in producerAlbums.Albums)
            {
                sb
                    .AppendLine($"-AlbumName: {album.AlbumName}")
                    .AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}")
                    .AppendLine($"-ProducerName: {album.ProducerName}")
                    .AppendLine($"-Songs:");

                int counter = 1;

                foreach (var song in album.Songs)
                {
                    sb
                        .AppendLine($"---#{counter++}")
                        .AppendLine($"---SongName: {song.SongName}")
                        .AppendLine($"---Price: {song.Price:F2}")
                        .AppendLine($"---Writer: {song.Writer}");
                }

                sb
                    .AppendLine($"-AlbumPrice: {album.AlbumPrice:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Include(s => s.Writer)
                .Include(s => s.Album)
                .ThenInclude(a => a.Producer)
                .Include(s => s.SongPerformers)
                .ThenInclude(sp => sp.Performer)
                .ToList()
                .Where(x => x.Duration.TotalSeconds > duration)
                .Select(song => new
                {
                    SongName = song.Name,
                    Writer = song.Writer.Name,
                    Performer = song.SongPerformers.Select(p => 
                        p.Performer.FirstName + " " + p.Performer.LastName)
                        .FirstOrDefault(),
                    AlbumProducer = song.Album.Producer.Name,
                    Duration = song.Duration
                })
                .OrderBy(x => x.SongName)
                .ThenBy(x => x.Writer)
                .ThenBy(x => x.Performer)
                .ToList();

            var sb = new StringBuilder();
            int counter = 1;

            foreach (var song in songs)
            {
                sb
                    .AppendLine($"-Song #{counter++}")
                    .AppendLine($"---SongName: {song.SongName}")
                    .AppendLine($"---Writer: {song.Writer}")
                    .AppendLine($"---Performer: {song.Performer}")
                    .AppendLine($"---AlbumProducer: {song.AlbumProducer}")
                    .AppendLine($"---Duration: {song.Duration.ToString("c")}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
