using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace DB_Connect
{
    class Program
    {
        static string connectionString =
        "Data Source=COMP11A1\\SQLEXPRESS;" +
        "Initial Catalog=DataBase;" +
        "Integrated Security=True;" +
        "TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n База товаров ");
                Console.WriteLine("1. Показать все товары");
                Console.WriteLine("2. Найти товар по ID");
                Console.WriteLine("3. Добавить товар");
                Console.WriteLine("4. Изменить товар");
                Console.WriteLine("5. Удалить товар");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            ListAllProducts();
                            break;
                        case "2":
                            FindProduct();
                            break;
                        case "3":
                            AddProduct();
                            break;
                        case "4":
                            UpdateProduct();
                            break;
                        case "5":
                            DeleteProduct();
                            break;
                        case "0":
                        case null:
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор.");
                            break;
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547)
                    {
                        Console.WriteLine("Нарушена связь между таблицами. " +
                            "При добавлении или изменении проверьте, существует ли BrandID. " +
                            "При удалении проверьте, не используется ли товар в другой таблице.");
                    }
                    else if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        Console.WriteLine("Запись с таким уникальным значением уже существует.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка SQL Server: " + ex.Message);
                    }
                }
                catch (OperationCanceledException)
                {
                    running = false;
                }

                if (running)
                {
                    Console.WriteLine("\nНажмите Enter для продолжения...");
                    Console.ReadLine();
                }
            }
        }

        static void ListAllProducts()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT ProductID, ProductName, ProductNumber, " +
                    "StandardCost, ListPrice, Color, BrandID " +
                    "FROM dbo.Products ORDER BY ProductID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    bool found = false;
                    while (reader.Read())
                    {
                        PrintProduct(reader);
                        found = true;
                    }

                    if (!found)
                        Console.WriteLine("Товаров пока нет.");
                }
            }
        }

        static void FindProduct()
        {
            int id = ReadPositiveInt("Введите ID товара: ");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT ProductID, ProductName, ProductNumber, " +
                    "StandardCost, ListPrice, Color, BrandID " +
                    "FROM dbo.Products WHERE ProductID = @id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            PrintProduct(reader);
                        else
                            Console.WriteLine("Товар не найден.");
                    }
                }
            }
        }

        static void AddProduct()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                bool autoId;
                using (SqlCommand check = new SqlCommand(
                    "SELECT COLUMNPROPERTY(OBJECT_ID('dbo.Products'), " +
                    "'ProductID', 'IsIdentity')", connection))
                {
                    autoId = Convert.ToInt32(check.ExecuteScalar()) == 1;
                }

                int id = 0;
                if (!autoId)
                    id = ReadPositiveInt("Введите ID нового товара: ");

                string sql;
                if (autoId)
                {
                    sql = "INSERT INTO dbo.Products " +
                        "(ProductName, ProductNumber, StandardCost, ListPrice, Color, BrandID) " +
                        "VALUES (@name, @number, @cost, @price, @color, @brand)";
                }
                else
                {
                    sql = "INSERT INTO dbo.Products " +
                        "(ProductID, ProductName, ProductNumber, StandardCost, ListPrice, Color, BrandID) " +
                        "VALUES (@id, @name, @number, @cost, @price, @color, @brand)";
                }

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (!autoId)
                        command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    ReadProductParameters(command);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Товар добавлен.");
                }
            }
        }

        static void UpdateProduct()
        {
            int id = ReadPositiveInt("Введите ID товара для изменения: ");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand check = new SqlCommand(
                    "SELECT COUNT(*) FROM dbo.Products WHERE ProductID = @id", connection))
                {
                    check.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    if (Convert.ToInt32(check.ExecuteScalar()) == 0)
                    {
                        Console.WriteLine("Товар не найден.");
                        return;
                    }
                }

                Console.WriteLine("Введите новые значения всех полей. ID не изменяется.");
                string sql = "UPDATE dbo.Products SET ProductName = @name, " +
                    "ProductNumber = @number, StandardCost = @cost, " +
                    "ListPrice = @price, Color = @color, BrandID = @brand " +
                    "WHERE ProductID = @id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    ReadProductParameters(command);
                    int rows = command.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Товар изменён." : "Товар не найден.");
                }
            }
        }

        static void DeleteProduct()
        {
            int id = ReadPositiveInt("Введите ID товара для удаления: ");
            Console.Write("Подтвердите удаление (да): ");
            string answer = ReadLine();
            if (!string.Equals(answer.Trim(), "да", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Удаление отменено.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(
                    "DELETE FROM dbo.Products WHERE ProductID = @id", connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    int rows = command.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Товар удалён." : "Товар не найден.");
                }
            }
        }

        static void PrintProduct(SqlDataReader reader)
        {
            string color = reader.IsDBNull(5) ? "не указан" : reader.GetString(5);
            Console.WriteLine(
                $"ID: {reader.GetInt32(0)}, " +
                $"Название: {reader.GetString(1)}, " +
                $"Номер: {reader.GetString(2)}, " +
                $"Себестоимость: {reader.GetDecimal(3):0.####}, " +
                $"Цена продажи: {reader.GetDecimal(4):0.####}, " +
                $"Цвет: {color}, BrandID: {reader.GetInt32(6)}");
        }

        static void ReadProductParameters(SqlCommand command)
        {
            string name = ReadText("Название товара: ", 100, false);
            string number = ReadText("Номер товара: ", 40, false);
            decimal cost = ReadMoney("Себестоимость: ");
            decimal price = ReadMoney("Цена продажи: ");
            string color = ReadText("Цвет (Enter — не указан): ", 20, true);
            int brand = ReadPositiveInt("ID существующего бренда: ");

            command.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = name;
            command.Parameters.Add("@number", SqlDbType.NVarChar, 40).Value = number;
            command.Parameters.Add("@cost", SqlDbType.Money).Value = cost;
            command.Parameters.Add("@price", SqlDbType.Money).Value = price;
            command.Parameters.Add("@color", SqlDbType.NVarChar, 20).Value =
                color.Length == 0 ? (object)DBNull.Value : color;
            command.Parameters.Add("@brand", SqlDbType.Int).Value = brand;
        }

        static string ReadLine()
        {
            string text = Console.ReadLine();
            if (text == null)
                throw new OperationCanceledException();
            return text;
        }

        static int ReadPositiveInt(string message)
        {
            while (true)
            {
                Console.Write(message);
                int value;
                if (int.TryParse(ReadLine(), out value) && value > 0)
                    return value;
                Console.WriteLine("Введите целое число больше нуля.");
            }
        }

        static decimal ReadMoney(string message)
        {
            while (true)
            {
                Console.Write(message);
                string text = ReadLine().Trim().Replace(',', '.');
                decimal value;
                if (decimal.TryParse(text,
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture, out value) &&
                    value >= 0 && value <= 922337203685477.5807m &&
                    decimal.Round(value, 4) == value)
                {
                    return value;
                }
                Console.WriteLine("Введите неотрицательную сумму в диапазоне money " +
                    "с максимум 4 знаками после запятой (например, 125,50).");
            }
        }

        static string ReadText(string message, int maxLength, bool allowEmpty)
        {
            while (true)
            {
                Console.Write(message);
                string text = ReadLine().Trim();
                if ((allowEmpty || text.Length > 0) && text.Length <= maxLength)
                    return text;
                Console.WriteLine("Введите " + (allowEmpty ? "не более " : "от 1 до ") +
                    maxLength + " символов.");
            }
        }
    }
}