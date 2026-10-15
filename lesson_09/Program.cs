using Course.Data;
var repository = new StudentRepositoryDapper(ConnectionFactory.GetConnectionString());
foreach (var student in await repository.GetAllAsync()) Console.WriteLine(student);
