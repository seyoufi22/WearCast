using System;
using Microsoft.Data.SqlClient;

namespace FixMigrationsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Extracted from user secrets
            string connectionString = "Server=localhost,1433;Database=WearCastDB;User Id=sa;Password=OpenMe@#;TrustServerCertificate=True;"; 

            Console.WriteLine("Connecting to the database to drop all tables...");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // This script disables constraints, drops all tables, and re-enables constraints.
                    string sqlScript = @"
                    EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT all'
                    EXEC sp_MSforeachtable 'DROP TABLE ?'
                    ";

                    using (SqlCommand command = new SqlCommand(sqlScript, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Successfully dropped all tables in the database.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
