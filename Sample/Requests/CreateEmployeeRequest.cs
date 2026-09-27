namespace Sample.Requests;

public class CreateEmployeeRequest
{
    public required string? Name { get; init; }

    public required int? Age { get; init; }

    public Department CurrentDepartment { get; set; }

    public List<Department> PastDepartments { get; init; } = new();
}

public enum Department
{
    Finance,
    Accounting,
    Legal
}