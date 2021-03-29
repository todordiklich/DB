namespace SoftJail.DataProcessor
{

    using Data;
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using System.Globalization;
    using SoftJail.Data.Models.Enums;
    using Newtonsoft.Json;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentCellsDTO = JsonConvert
                .DeserializeObject<ICollection<DepartmentCellsDTO>>(jsonString);

            var sb = new StringBuilder();

            var validDepartments = new List<Department>();

            foreach (var departmentCells in departmentCellsDTO)
            {
                if (!IsValid(departmentCells) ||
                    !departmentCells.Cells.All(IsValid) ||
                    departmentCells.Cells.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Department department = new Department()
                {
                    Name = departmentCells.Name,
                    Cells = departmentCells.Cells.Select(c => new Cell
                    {
                        CellNumber = c.CellNumber,
                        HasWindow = c.HasWindow
                    })
                        .ToList()
                };

                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
                validDepartments.Add(department);
            }

            context.Departments.AddRange(validDepartments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var prisoners = new List<Prisoner>();

            var prisonerMails = JsonConvert
                .DeserializeObject<ICollection<ImportPrisonersMailsDTO>>(jsonString);

            foreach (var currentPrisoner in prisonerMails)
            {
                if (!IsValid(currentPrisoner) ||
                    !currentPrisoner.Mails.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                //dd/MM/yyyy
                var isValidReleaseDate = DateTime.TryParseExact(
                    currentPrisoner.ReleaseDate,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime releaseDate);

                var incarcerationDate = DateTime.ParseExact(
                    currentPrisoner.IncarcerationDate,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture);


                var prisoner = new Prisoner
                {
                    FullName = currentPrisoner.FullName,
                    Nickname = currentPrisoner.Nickname,
                    Age = currentPrisoner.Age,
                    Bail = currentPrisoner.Bail,
                    CellId = currentPrisoner.CellId,
                    ReleaseDate = isValidReleaseDate ? (DateTime?)releaseDate : null,
                    IncarcerationDate = incarcerationDate,
                    Mails = currentPrisoner.Mails.Select(m => new Mail
                    {
                        Sender = m.Sender,
                        Address = m.Address,
                        Description = m.Description
                    })
                    .ToList()
                };

                prisoners.Add(prisoner);

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var importOfficersPrisoners = XmlConverter.Deserializer<ImportOfficersPrisonersDTO>(xmlString, "Officers");

            var sb = new StringBuilder();
            var validOfficers = new List<Officer>();

            foreach (var officerDTO in importOfficersPrisoners)
            {
                if (!IsValid(officerDTO))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var validOfficer = new Officer
                {
                    FullName = officerDTO.FullName,
                    Salary = officerDTO.Salary,
                    Position = Enum.Parse<Position>(officerDTO.Position),
                    Weapon = Enum.Parse<Weapon>(officerDTO.Weapon),
                    DepartmentId = officerDTO.DepartmentId,
                    OfficerPrisoners = officerDTO.Prisoners.Select(p => new OfficerPrisoner
                    {
                        PrisonerId = p.Id
                    }).ToArray()
                };

                validOfficers.Add(validOfficer);
                sb.AppendLine($"Imported {validOfficer.FullName} ({validOfficer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(validOfficers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}