using System;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();

            var result = RemoveTown(db);

            Console.WriteLine(result);
        }


        // Problem 3
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var empFullInfo = context.Employees.Select(x => new
            {
                x.FirstName,
                x.LastName,
                x.MiddleName,
                x.JobTitle,
                x.Salary,
                x.EmployeeId
            })
                .OrderBy(x => x.EmployeeId)
                .ToList();

            var sb = new StringBuilder();

            foreach (var employee in empFullInfo)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }


        //Problem 4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.Salary > 50_000)
                .Select(x => new
                {
                    x.FirstName,
                    x.Salary
                })
                .ToList()
                .OrderBy(x => x.FirstName);

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }


        //Problem 5
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.Department.Name == "Research and Development")
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    DepartmentName = x.Department.Name,
                    x.Salary
                })
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }


        //Problem 6
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var newAddress = new Address() 
            {
                TownId = 4,
                AddressText = "Vitoshka 15"
            };

            var employeeByName = context.Employees
                .FirstOrDefault(x => x.LastName == "Nakov");

            employeeByName.Address = newAddress;
            context.SaveChanges();

            var employees = context.Employees
                .Select(x => new
                {
                    addressId = x.Address.AddressId,
                    addressText = x.Address.AddressText
                })
                .ToList()
                .OrderByDescending(x => x.addressId)
                .Take(10);

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.addressText}");
            }

            return sb.ToString().TrimEnd();
        }


        //Problem 7
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employeesProjects = context.EmployeesProjects
                .Where(e => e.Project.StartDate.Year >= 2001
                             && e.Project.StartDate.Year <= 2003)
                .Select(x => new
                {
                    empFirstName = x.Employee.FirstName,
                    empLastName = x.Employee.LastName,
                    managerFirstName = x.Employee.Manager.FirstName,
                    managerLastName = x.Employee.Manager.LastName,
                    projects = x.Employee.EmployeesProjects                    
                    .Select(p => p.Project)
                    .ToList()
                })
                .ToList()
                .Take(10);

            var sb = new StringBuilder();

            foreach (var employee in employeesProjects)
            {
                sb.AppendLine($"{employee.empFirstName} {employee.empLastName} - Manager: {employee.managerFirstName} {employee.managerLastName}");

                foreach (var project in employee.projects)
                {
                    var startDate = project.StartDate.ToString("M/d/yyyy h:mm:ss tt");
                    var endDate = project.EndDate == null
                        ? "not finished"
                        : project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt");

                    sb.AppendLine($"--{project.Name} - {startDate} - {endDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }


        //Problem 8
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var adresses = context.Addresses
                .Select(x => new
                {
                    townName = x.Town.Name,
                    addressText = x.AddressText,
                    peopleCount = x.Employees.Count
                })
                .ToList()
                .OrderByDescending(e => e.peopleCount)
                .ThenBy(t => t.townName)
                .ThenBy(a => a.addressText)
                .Take(10);

            var sb = new StringBuilder();

            foreach (var address in adresses)
            {
                sb.AppendLine($"{address.addressText}, {address.townName} - {address.peopleCount} employees");
            }

            return sb.ToString().TrimEnd();
        }


        //Problem 9
        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Where(x => x.EmployeeId == 147)
                .Select(e => new
                {
                    firstName = e.FirstName,
                    lastName = e.LastName,
                    jobTitle = e.JobTitle,
                    projects = e.EmployeesProjects
                    .Select(p => p.Project)
                    .OrderBy(y => y.Name)
                    .ToList()
                })
                .FirstOrDefault();


            var sb = new StringBuilder();

            sb.AppendLine($"{employee.firstName} {employee.lastName} - {employee.jobTitle}");

            foreach (var project in employee.projects)
            {
                sb.AppendLine($"{project.Name}");
            }

            return sb.ToString().TrimEnd();
        }


        //Problem 10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(x => x.Employees.Count > 5)
                .Select(x => new
                {
                    departmentName = x.Name,
                    managerFirstName = x.Manager.FirstName,
                    managerLastName = x.Manager.LastName,
                    employees = x.Employees
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    })
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList()
                })
                .ToList()
                .OrderBy(x => x.employees.Count)
                .ThenBy(x => x.departmentName);

            var sb = new StringBuilder();

            foreach (var department in departments)
            {
                sb.AppendLine($"{department.departmentName} - {department.managerFirstName} {department.managerLastName}");

                foreach (var emp in department.employees)
                {
                    sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }


        //Problem 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    x.StartDate
                })
                .OrderByDescending(p => p.StartDate)
                .ToList()
                .Take(10)
                .OrderBy(x => x.Name);

            var sb = new StringBuilder();

            foreach (var project in projects)
            {
                sb
                    .AppendLine($"{project.Name}")
                    .AppendLine($"{project.Description}")
                    .AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt"));

            }

            return sb.ToString().TrimEnd();
        }


        //Problem 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => new[] { "Engineering", "Tool Design", "Marketing", "Information Services" }
                        .Contains(e.Department.Name))
                .ToList();

            foreach (var employee in employees)
            {
                employee.Salary *= 1.12m;
            }

            context.SaveChanges();

            var employeesResult = context.Employees
                .Where(e => new[] { "Engineering", "Tool Design", "Marketing", "Information Services" }
                        .Contains(e.Department.Name))
                .ToList()
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName);

            var sb = new StringBuilder();

            foreach (var employee in employeesResult)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");

            }

            return sb.ToString().TrimEnd();
        }


        //Problem 13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var empluyees = context.Employees
                .Where(e => EF.Functions.Like(e.FirstName, "sa%"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToList()
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName);

            var sb = new StringBuilder();

            foreach (var employee in empluyees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");

            }

            return sb.ToString().TrimEnd();
        }


        //Problem 14
        public static string DeleteProjectById(SoftUniContext context)
        {
            var project = context.Projects.FirstOrDefault(p => p.ProjectId == 2);

            var employeesProjects = context.EmployeesProjects
                .Where(p => p.ProjectId == project.ProjectId)
                .ToList();

            foreach (var empProj in employeesProjects)
            {
                context.EmployeesProjects.Remove(empProj);
            }

            context.Projects.Remove(project);
            context.SaveChanges();

            var projects = context.Projects.ToList().Take(10);

            var sb = new StringBuilder();

            foreach (var proj in projects)
            {
                sb.AppendLine($"{proj.Name}");

            }

            return sb.ToString().TrimEnd();
        }


        //Problem 15
        public static string RemoveTown(SoftUniContext context) 
        {
            var town = context.Towns.FirstOrDefault(t => t.Name == "Seattle");

            var addressesByTown = context.Addresses
                .Where(a => a.TownId == town.TownId)
                .ToList();

            var employeesByAddress = context.Employees
                .Where(e => addressesByTown.Contains(e.Address))
                .ToList();

            foreach (var employee in employeesByAddress)
            {
                employee.Address = null;
            }

            foreach (var address in addressesByTown)
            {
                context.Addresses.Remove(address);
            }

            context.Remove(town);

            context.SaveChanges();

            return $"{addressesByTown.Count} addresses in Seattle were deleted";
        }
    }
}
