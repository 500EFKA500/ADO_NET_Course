using Course.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new StudentRepositoryDapper(
    builder.Configuration.GetConnectionString("College") ?? ConnectionFactory.GetConnectionString()));
builder.Services.AddSingleton(new GroupRepositoryDapper(
    builder.Configuration.GetConnectionString("College") ?? ConnectionFactory.GetConnectionString()));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/students", async (StudentRepositoryDapper repository) =>
    Results.Ok(await repository.GetAllAsync()))
    .WithName("GetStudents");

app.MapGet("/api/students/{id:int}", async (int id, StudentRepositoryDapper repository) =>
    await repository.FindAsync(id) is { } student ? Results.Ok(student) : Results.NotFound())
    .WithName("GetStudentById");

app.MapGet("/api/groups", async (GroupRepositoryDapper repository) =>
    Results.Ok(await repository.GetAllAsync()))
    .WithName("GetGroups");

app.Run();
