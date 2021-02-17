using System;
using Microsoft.Data.SqlClient;

namespace P02
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.;Database=MinionsDB;Integrated Security=true";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
    FROM Villains AS v 
    JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
GROUP BY v.Id, v.Name 
  HAVING COUNT(mv.VillainId) > 3 
ORDER BY COUNT(mv.VillainId)";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Name"] as string;
                            int count = (int)reader["MinionsCount"];
                            Console.WriteLine($"{name} - {count}");
                        }
                    }

                }
            }
        }
    }
}
