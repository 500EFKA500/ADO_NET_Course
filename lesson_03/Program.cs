using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Connect
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
                "Data Source=COMP11A1\\SQLEXPRESS;" +
                "Initial Catalog=DataBase;" +
                "Integrated Security=True;" +
                "TrustServerCertificate=True;"
            ;
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string sql_command = "SELECT TOP 10 * FROM Products";

            SqlCommand command = new SqlCommand(sql_command, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int ProductID = reader.GetInt32(0);
                    string ProductName = reader.GetString(1);
                    string ProductNumber = reader.GetString(2);
                    decimal StandardCost = reader.GetDecimal(3);
                    decimal ListPrice = reader.GetDecimal(4);

                    Console.WriteLine(
                        $"Id {ProductID}, " +
                        $"Имя {ProductName}, " +
                        $"Номер {ProductNumber}, " +
                        $"Цена {StandardCost}, " +
                        $"Начальная цена {ListPrice}");
                }

                Console.ReadKey();
            }
        }
    }
}
