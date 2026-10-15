namespace Course.Data;

public sealed record Group(int Id, string Name);

public sealed record Student(int Id, string FirstName, string LastName, int Age, int GroupId, string? GroupName = null)
{
    public override string ToString() => $"{Id}: {FirstName} {LastName}, {Age} лет, группа {GroupName ?? GroupId.ToString()}";
}
