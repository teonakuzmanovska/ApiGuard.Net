namespace Sample.Requests;

public class CreateEmployeeRequest
{
    public required string? Name { get; init; }

    public required int? Age { get; init; }

    public Department CurrentDepartment { get; init; }

    public List<Department> PastDepartments
    {
        get => field;
        init => field = value;
    } = new();
}

public enum Department
{
    Finance,
    Accounting,
    Legal
}