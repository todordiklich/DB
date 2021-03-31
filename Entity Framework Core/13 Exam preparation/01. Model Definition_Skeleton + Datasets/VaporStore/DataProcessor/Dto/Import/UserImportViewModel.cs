using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class UserImportViewModel
    {
        public UserImportViewModel()
        {
            this.Cards = new HashSet<CardImportViewModel>();
        }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+")]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Range(3, 103)]
        public int Age { get; set; }
        public ICollection<CardImportViewModel> Cards { get; set; }
    }
}
//•	Username – text with length [3, 20] (required)
//•	FullName – text, which has two words, consisting of Latin letters. Both start with an upper letter and are followed by lower letters. The two words are separated by a single space (ex. "John Smith") (required)
//•	Email – text(required)
//•	Age – integer in the range[3, 103] (required)
//•	Cards – collection of type Card

//{
//    "FullName": "",
//    "Username": "invalid",
//    "Email": "invalid@invalid.com",
//    "Age": 20,
//    "Cards": [
//      {
//        "Number": "1111 1111 1111 1111",
//        "CVC": "111",
//        "Type": "Debit"
//      }
//    ]
//  },
