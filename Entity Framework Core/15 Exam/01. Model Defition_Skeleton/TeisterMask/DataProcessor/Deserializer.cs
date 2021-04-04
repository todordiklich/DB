namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var projectsDTO = XmlConverter.Deserializer<ProjectImportViewModel>(xmlString, "Projects");

            foreach (var projectDTO in projectsDTO)
            {
                var isProjectOpendateParsed = DateTime.TryParseExact(projectDTO.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime projectOpendate);

                var isProjectDuedateParsed = DateTime.TryParseExact(projectDTO.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime projectDuedate);

                if (!IsValid(projectDTO) || isProjectOpendateParsed == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validTasks = new List<TaskImportViewModel>();

                foreach (var taskDTO in projectDTO.Tasks)
                {
                    var isTaskOpendateParsed = DateTime.TryParseExact(taskDTO.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskOpendate);

                    var isTaskDuedateParsed = DateTime.TryParseExact(taskDTO.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDuedate);


                    if (!IsValid(taskDTO) 
                        || isTaskOpendateParsed == false 
                        || isTaskDuedateParsed == false 
                        || projectOpendate > taskOpendate 
                        || taskOpendate > taskDuedate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    else if (isProjectDuedateParsed)
                    {
                        if (projectDuedate < taskDuedate || projectOpendate > projectDuedate)
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }
                    }

                    validTasks.Add(taskDTO);
                }

                var project = new Project
                {
                    Name = projectDTO.Name,
                    OpenDate = projectOpendate,
                    DueDate = isProjectDuedateParsed ? (DateTime?)projectDuedate : null,
                    Tasks = validTasks.Select(t => new Task
                    {
                        Name = t.Name,
                        OpenDate = DateTime.ParseExact(t.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DueDate = DateTime.ParseExact(t.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ExecutionType = (ExecutionType)t.ExecutionType,
                        LabelType = (LabelType)t.LabelType
                    }).ToArray()
                };

                context.Add(project);
                sb.AppendLine(String.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var employeesDTO = JsonConvert.DeserializeObject<ICollection<EmployeeImportViewModel>>(jsonString);

            foreach (var employeeDTO in employeesDTO)
            {
                var allTasksIds = context.Tasks.Select(t => t.Id).ToList();

                if (!IsValid(employeeDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currentEmployeeTasks = employeeDTO.Tasks.Distinct().ToList();
                var validTaks = new List<int>();

                foreach (var task in currentEmployeeTasks)
                {
                    if (allTasksIds.Contains(task))
                    {
                        validTaks.Add(task);
                    }
                    else
                    {
                        sb.AppendLine(ErrorMessage);
                    }
                }


                var employee = new Employee
                {
                    Username = employeeDTO.Username,
                    Email = employeeDTO.Email,
                    Phone = employeeDTO.Phone,
                };

                foreach (var taskId in validTaks)
                {
                    var task = context.Tasks.Where(t => t.Id == taskId).FirstOrDefault()
                        ?? new Task { Id = taskId };
                    employee.EmployeesTasks.Add(new EmployeeTask { Task = task });
                }

                context.Employees.Add(employee);
                sb.AppendLine(String.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
                context.SaveChanges();
            }

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