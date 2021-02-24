using System;
using System.Linq;
using System.Text;
using SoftUni.Data;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();
            var result = GetEmployeesFullInformation(db);
            Console.WriteLine(result);
        }

        // problem 3
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
    }
}
