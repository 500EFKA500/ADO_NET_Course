using Dapper;
using Microsoft.Data.SqlClient;

namespace Course.Data;

public sealed class StudentRepositoryDapper(string connectionString)
{
    public async Task<IReadOnlyList<Student>> GetAllAsync()
    {
        const string sql = """
            SELECT s.Id, s.FirstName, s.LastName, s.Age, s.GroupId, g.Name AS GroupName
            FROM dbo.Students s JOIN dbo.Groups g ON g.Id = s.GroupId ORDER BY s.LastName, s.FirstName
            """;
        await using var connection = new SqlConnection(connectionString);
        return (await connection.QueryAsync<Student>(sql)).AsList();
    }

    public async Task<Student?> FindAsync(int id)
    {
        const string sql = """
            SELECT s.Id, s.FirstName, s.LastName, s.Age, s.GroupId, g.Name AS GroupName
            FROM dbo.Students s JOIN dbo.Groups g ON g.Id = s.GroupId WHERE s.Id = @id
            """;
        await using var connection = new SqlConnection(connectionString);
        return await connection.QueryFirstOrDefaultAsync<Student>(sql, new { id });
    }

    public async Task<int> CreateAsync(Student student)
    {
        const string sql = """
            INSERT dbo.Students(FirstName, LastName, Age, GroupId)
            OUTPUT INSERTED.Id VALUES (@FirstName, @LastName, @Age, @GroupId);
            """;
        await using var connection = new SqlConnection(connectionString);
        return await connection.QuerySingleAsync<int>(sql, student);
    }
}

public sealed class GroupRepositoryDapper(string connectionString)
{
    public async Task<IReadOnlyList<Group>> GetAllAsync()
    {
        await using var connection = new SqlConnection(connectionString);
        return (await connection.QueryAsync<Group>("SELECT Id, Name FROM dbo.Groups ORDER BY Name")).AsList();
    }
}
