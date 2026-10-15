using Course.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new StudentRepositoryDapper(
    builder.Configuration.GetConnectionString("College") ?? ConnectionFactory.GetConnectionString()));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/students", async (StudentRepositoryDapper repository) =>
    Results.Ok(await repository.GetAllAsync()))
    .WithName("GetStudents");

app.MapGet("/api/message", () => Results.Ok(new { message = "Подключение к SQL Server готово" }))
    .WithName("GetMessage");

app.Run();
