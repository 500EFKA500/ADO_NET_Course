using Course.Data;
var repository = new StudentRepositoryAdo(ConnectionFactory.GetConnectionString());
Console.Write("Имя или фамилия для поиска (Enter — все): ");
var query = Console.ReadLine();
foreach (var student in await repository.GetAllAsync(string.IsNullOrWhiteSpace(query) ? null : query)) Console.WriteLine(student);
