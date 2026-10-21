using College_DB.Data;
using College_DB.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("CollegeDB")
    ?? throw new InvalidOperationException("ConnectionStrings:CollegeDB is required.");
builder.Services.AddSingleton(new StudentRepository(connectionString));
builder.Services.AddSingleton(new GroupRepository(connectionString));
var app = builder.Build();
DatabaseInitializer.Initialize(connectionString);
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapGet("/students/all", (StudentRepository repo) => Results.Ok(repo.GetAllStudents()));
app.MapGet("/groups/all", (GroupRepository repo) => Results.Ok(repo.GetAllGroups()));
app.MapGet("/group/{id:int}", (int id, GroupRepository repo) =>
    repo.GetGroupById(id.ToString()) is { } group ? Results.Ok(group) : Results.NotFound());
app.MapGet("/student/{id:int}", (int id, StudentRepository repo) =>
    repo.GetStudentById(id.ToString()) is { } student ? Results.Ok(student) : Results.NotFound());
app.MapPost("/students/add", (Student student, StudentRepository students, GroupRepository groups) =>
{
    if (string.IsNullOrWhiteSpace(student.FirstName) || string.IsNullOrWhiteSpace(student.LastName)
        || student.FirstName.Length > 100 || student.LastName.Length > 100
        || student.Age < 1 || student.Age > 120)
        return Results.BadRequest(new { error = "Укажите имя, фамилию (до 100 символов) и возраст от 1 до 120." });
    if (groups.GetGroupById(student.GroupId.ToString()) is null)
        return Results.BadRequest(new { error = "Выберите существующую группу." });
    student.FirstName = student.FirstName.Trim();
    student.LastName = student.LastName.Trim();
    students.CreateStudent(student);
    return Results.Ok(student);
});
app.Run();
