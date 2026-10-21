using Dapper;
using Microsoft.Data.SqlClient;
namespace College_DB.Data;

internal static class DatabaseInitializer
{
    public static void Initialize(string connectionString)
    {
        var settings = new SqlConnectionStringBuilder(connectionString);
        var database = settings.InitialCatalog;
        if (string.IsNullOrWhiteSpace(database))
            throw new InvalidOperationException("Укажите имя базы данных.");
        settings.InitialCatalog = "master";
        using (var master = new SqlConnection(settings.ConnectionString))
        {
            var quotedName = "[" + database.Replace("]", "]]") + "]";
            master.Execute($"IF DB_ID(@database) IS NULL CREATE DATABASE {quotedName}", new { database });
        }
        using var connection = new SqlConnection(connectionString);
        connection.Execute("""
            IF OBJECT_ID(N'dbo.Groups', N'U') IS NULL
            BEGIN
                CREATE TABLE dbo.Groups (
                    GroupId int IDENTITY PRIMARY KEY,
                    GroupName nvarchar(100) NOT NULL);
                INSERT INTO dbo.Groups (GroupName) VALUES (N'ИС-101'), (N'ИС-102'), (N'ПР-201');
            END;
            IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
                CREATE TABLE dbo.Students (
                    StudentId int IDENTITY PRIMARY KEY,
                    FirstName nvarchar(100) NOT NULL,
                    LastName nvarchar(100) NOT NULL,
                    Age int NOT NULL CHECK (Age BETWEEN 1 AND 120),
                    GroupId int NOT NULL REFERENCES dbo.Groups(GroupId));
            """);
    }
}
