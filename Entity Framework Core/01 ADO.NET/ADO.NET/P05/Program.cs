using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace P05
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.;Database=MinionsDB;Integrated Security=true";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string countryName = Console.ReadLine();

                string townsChangeQuery = @"UPDATE Towns
   SET Name = UPPER(Name)
 WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                using (var command = new SqlCommand(townsChangeQuery, connection))
                {
                    command.Parameters.AddWithValue("@countryName", countryName);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        Console.WriteLine("No town names were affected.");
                    }
                    else
                    {
                        Console.WriteLine($"{rowsAffected} town names were affected.");

                        string affectedTownsQuery = @"SELECT t.Name 
   FROM Towns as t
   JOIN Countries AS c ON c.Id = t.CountryCode
  WHERE c.Name = @countryName";

                        using (var townsCmd = new SqlCommand(affectedTownsQuery, connection))
                        {
                            townsCmd.Parameters.AddWithValue("@countryName", countryName);
                            var towns = new List<string>();

                            using (var reader = townsCmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    towns.Add(reader[0] as string);
                                }
                            }

                            Console.WriteLine(string.Join(", ", towns));
                        }
                    }
                }
            }
        }
    }
}
