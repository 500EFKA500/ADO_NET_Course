using Course.Data;
var groups = new GroupRepositoryAdo(ConnectionFactory.GetConnectionString());
Console.Write("Название новой группы: ");
var name = Console.ReadLine()?.Trim();
if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Название обязательно."); return; }
Console.WriteLine($"Группа создана: Id = {await groups.CreateAsync(name)}");
