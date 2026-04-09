using System.Configuration;

public class EmployeeRepository
{

    private readonly string _connectionString;

    public EmployeeRepository()
    {
        _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }


    public bool GetEmployeeById(int id)
    {
        // Simulate database access and return a dummy employee record
        return id == 1; // Assume employee with ID 1 exists
    }
    public void EnableEmployee(int id, int enable)
    {
        // Simulate updating the employee's enabled status in the database
        // In a real implementation, you would execute a SQL UPDATE statement here
    }
}