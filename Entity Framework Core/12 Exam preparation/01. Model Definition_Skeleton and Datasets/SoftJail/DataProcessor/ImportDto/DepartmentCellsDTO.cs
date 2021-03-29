using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class DepartmentCellsDTO
    {
        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; }
        public ICollection<ImportCellDTO> Cells { get; set; }
    }

    public class ImportCellDTO
    {
        [Range(1, 1000)]
        public int CellNumber { get; set; }
        public bool HasWindow { get; set; }
    }
}
//•	Name – text with min length 3 and max length 25 (required)

//•	CellNumber – integer in the range [1, 1000] (required)
//•	HasWindow – bool(required)

//{
//    "Name": "",
//    "Cells": [
//      {
//        "CellNumber": 101,
//        "HasWindow": true
//      },
//      {
//        "CellNumber": 102,
//        "HasWindow": false
//      }
//    ]
//  },