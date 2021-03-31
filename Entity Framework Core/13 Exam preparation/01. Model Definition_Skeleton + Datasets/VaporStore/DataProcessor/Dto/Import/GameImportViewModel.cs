using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class GameImportViewModel
    {
        public GameImportViewModel()
        {
            this.Tags = new HashSet<string>();
        }

        [Required]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        public string Developer { get; set; }

        [Required]
        public string Genre { get; set; }

        public ICollection<string> Tags { get; set; }
    }
}
//•	Name – text (required)
//•	Price – decimal(non - negative, minimum value: 0)(required)
//•	ReleaseDate – Date(required)
//•	Developer – the game’s developer (required)
//•	Genre – the game’s genre (required)
//•	GameTags - collection of type GameTag. Each game must have at least one tag.

//{
//	"Name": "PLAYERUNKNOWN'S BATTLEGROUNDS",
//	"Price": 29.99,
//	"ReleaseDate": "2017-12-21",
//	"Developer": "PUBG Corporation",
//	"Genre": "Violent",
//	"Tags": [
//		"Multi-player",
//		"Online Multi-Player",
//		"Stats"
//	]
//	},