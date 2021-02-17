using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace P07
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.;Database=MinionsDB;Integrated Security=true";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string minionsQuery = @"SELECT Name FROM Minions";

                using (var command = new SqlCommand(minionsQuery, connection))
                {
                    var minions = new List<string>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            minions.Add(reader[0] as string);
                        }
                    }

                    for (int i = 0; i < minions.Count / 2 ; i++)
                    {
                        Console.WriteLine(minions[i]);
                        Console.WriteLine(minions[minions.Count - 1 - i]);
                    }
                    if (minions.Count % 2 !=0)
                    {
                        Console.WriteLine(minions[minions.Count / 2]);
                    }
                }
            }
        }
    }
}
