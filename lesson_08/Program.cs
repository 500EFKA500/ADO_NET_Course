using Course.Data;
var repository = new GroupRepositoryAdo(ConnectionFactory.GetConnectionString());
foreach (var group in await repository.GetAllAsync()) Console.WriteLine($"{group.Id}: {group.Name}");
