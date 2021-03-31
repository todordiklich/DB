using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class CardImportViewModel
    {
        [Required]
        [RegularExpression("(([0-9]{4} ){3}[0-9]{4})")]
        public string Number { get; set; }

        [Required]
        [RegularExpression("[0-9]{3}")]
        public string CVC { get; set; }

        [Required]
        public CardType? Type { get; set; }
    }
}
//•	Number – text, which consists of 4 pairs of 4 digits, separated by spaces (ex. “1234 5678 9012 3456”) (required)
//•	Cvc – text, which consists of 3 digits (ex. “123”) (required)
//•	Type – enumeration of type CardType, with possible values (“Debit”, “Credit”) (required)

//"Number": "1111 1111 1111 1111",
//"CVC": "111",
//"Type": "Debit"