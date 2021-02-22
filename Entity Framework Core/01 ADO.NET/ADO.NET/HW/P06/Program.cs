using System;
using Microsoft.Data.SqlClient;

namespace P06
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.;Database=MinionsDB;Integrated Security=true";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string villainQuery = @"SELECT Name FROM Villains WHERE Id = @villainId";
                int id = int.Parse(Console.ReadLine());

                using (var villainCommand = new SqlCommand(villainQuery, connection))
                {
                    villainCommand.Parameters.AddWithValue("@villainId", id);
                    string villainName = villainCommand.ExecuteScalar() as string;

                    if (villainName == null)
                    {
                        Console.WriteLine("No such villain was found.");
                    }
                    else
                    {
                        string minionsReleasedQuery = @"DELETE FROM MinionsVillains 
      WHERE VillainId = @villainId";

                        int releasedCount = 0;
                        using (var minionsCommand = new SqlCommand(minionsReleasedQuery, connection))
                        {
                            minionsCommand.Parameters.AddWithValue("@villainId", id);
                            releasedCount = minionsCommand.ExecuteNonQuery();
                        }
                        string villainDeleteQuery = @"DELETE FROM Villains
      WHERE Id = @villainId";

                        using (var villainDeleteCommand = new SqlCommand(villainDeleteQuery, connection))
                        {
                            villainDeleteCommand.Parameters.AddWithValue("@villainId", id);
                            villainDeleteCommand.ExecuteNonQuery();

                            Console.WriteLine($"{villainName} was deleted.");
                            Console.WriteLine($"{releasedCount} minions were released.");
                        }
                    }
                }
            }
        }
    }
}
