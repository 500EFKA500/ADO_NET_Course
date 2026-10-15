using Course.Data;
var students = await new StudentRepositoryAdo(ConnectionFactory.GetConnectionString()).GetAllAsync();
Console.WriteLine(students.Count == 0 ? "Студентов пока нет." : string.Join(Environment.NewLine, students));
