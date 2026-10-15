using Course.Data;
var repository = new StudentRepositoryDapper(ConnectionFactory.GetConnectionString());
Console.Write("Имя: "); var firstName = Console.ReadLine()?.Trim();
Console.Write("Фамилия: "); var lastName = Console.ReadLine()?.Trim();
Console.Write("Возраст: "); var ageText = Console.ReadLine();
Console.Write("Id группы: "); var groupText = Console.ReadLine();
if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || !int.TryParse(ageText, out var age) || !int.TryParse(groupText, out var groupId))
{ Console.WriteLine("Проверьте введённые значения."); return; }
var id = await repository.CreateAsync(new Student(0, firstName, lastName, age, groupId));
Console.WriteLine($"Студент создан, Id = {id}");
