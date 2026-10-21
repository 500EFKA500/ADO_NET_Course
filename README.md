app.MapPost("/students/add",
    (Student student, StudentRepository students, GroupRepository groups) =>
{
    if (string.IsNullOrWhiteSpace(student.FirstName) ||
        string.IsNullOrWhiteSpace(student.LastName) ||
        student.Age < 1 || student.Age > 120)
    {
        return Results.BadRequest("Проверьте имя, фамилию и возраст.");
    }

    if (groups.GetGroupById(student.GroupId.ToString()) is null)
    {
        return Results.BadRequest("Такой группы не существует.");
    }

    students.CreateStudent(student);

    return Results.Ok(student);
});
