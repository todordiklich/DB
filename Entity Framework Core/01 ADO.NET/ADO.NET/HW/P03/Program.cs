using System;
using Microsoft.Data.SqlClient;

namespace P03
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.;Database=MinionsDB;Integrated Security=true";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string nameQuery = @"SELECT Name FROM Villains WHERE Id = @Id";
                int id = int.Parse(Console.ReadLine());

                using (var nameCommand = new SqlCommand(nameQuery, connection))
                {
                    nameCommand.Parameters.AddWithValue("@Id", id);
                    string villainName = nameCommand.ExecuteScalar() as string;

                    if (villainName == null)
                    {
                        Console.WriteLine($"No villain with ID {id} exists in the database.");
                    }
                    else
                    {
                        Console.WriteLine($"Villain: {villainName}");

                        string minionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";

                        using (var minionsCommand = new SqlCommand(minionsQuery, connection))
                        {
                            minionsCommand.Parameters.AddWithValue("@Id", id);
                            using (var reader = minionsCommand.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"{reader[0]}. {reader[1]} {reader[2]}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("(no minions)");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
