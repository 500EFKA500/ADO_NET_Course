using Microsoft.Data.SqlClient;

namespace Course.Data;

public sealed class StudentRepositoryAdo(string connectionString)
{
    public async Task<IReadOnlyList<Student>> GetAllAsync(string? name = null)
    {
        const string sql = """
            SELECT s.Id, s.FirstName, s.LastName, s.Age, s.GroupId, g.Name AS GroupName
            FROM dbo.Students s INNER JOIN dbo.Groups g ON g.Id = s.GroupId
            WHERE @name IS NULL OR s.FirstName LIKE N'%' + @name + N'%' OR s.LastName LIKE N'%' + @name + N'%'
            ORDER BY s.LastName, s.FirstName;
            """;
        var students = new List<Student>();
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@name", (object?)name ?? DBNull.Value);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            students.Add(new Student(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetString(5)));
        return students;
    }

    public async Task<int> CreateAsync(string firstName, string lastName, int age, int groupId)
    {
        const string sql = "INSERT dbo.Students(FirstName, LastName, Age, GroupId) OUTPUT INSERTED.Id VALUES (@firstName, @lastName, @age, @groupId);";
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@firstName", firstName);
        command.Parameters.AddWithValue("@lastName", lastName);
        command.Parameters.AddWithValue("@age", age);
        command.Parameters.AddWithValue("@groupId", groupId);
        return (int)(await command.ExecuteScalarAsync())!;
    }
}

public sealed class GroupRepositoryAdo(string connectionString)
{
    public async Task<IReadOnlyList<Group>> GetAllAsync()
    {
        var groups = new List<Group>();
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand("SELECT Id, Name FROM dbo.Groups ORDER BY Name", connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) groups.Add(new Group(reader.GetInt32(0), reader.GetString(1)));
        return groups;
    }

    public async Task<int> CreateAsync(string name)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand("INSERT dbo.Groups(Name) OUTPUT INSERTED.Id VALUES (@name)", connection);
        command.Parameters.AddWithValue("@name", name);
        return (int)(await command.ExecuteScalarAsync())!;
    }
}
