namespace Course.Data;

public static class ConnectionFactory
{
    public const string DefaultConnectionString =
        "Server=(localdb)\\MSSQLLocalDB;Database=model;Trusted_Connection=True;TrustServerCertificate=True";

    public static string GetConnectionString() =>
        Environment.GetEnvironmentVariable("COLLEGE_DB_CONNECTION") ?? DefaultConnectionString;
}
