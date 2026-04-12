using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmployeeService.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _connectionString;

    #region SQL Queries
    private const string GetAllEmployeesQuery = "SELECT ID, Name, ManagerID, Enable FROM Employee";
    private const string GetManagerEmployeesQuery = @"
        WITH EmployeeTree AS (
            SELECT ID, Name, ManagerID, Enable 
            FROM Employee 
            WHERE ID = @ID
        
            UNION ALL
        
            SELECT e.ID, e.Name, e.ManagerID, e.Enable
            FROM Employee e
            JOIN EmployeeTree et ON e.ManagerID = et.Id
        )
        SELECT ID, Name, ManagerID, Enable FROM EmployeeTree";
    private const string EnableEmployeeQuery = @"
        UPDATE Employee
        SET Enable = @Enable
        WHERE ID = @ID";
    #endregion

    public EmployeeRepository()
    {
        _connectionString = ConfigurationManager.ConnectionStrings["Employee"].ConnectionString;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllUsersAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = new SqlCommand(GetAllEmployeesQuery, connection))
            {
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var employees = new List<EmployeeDto>();
                    while (await reader.ReadAsync())
                    {
                        employees.Add(new EmployeeDto
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ManagerID = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                            Enable = reader.GetBoolean(3)
                        });
                    }
                    return employees;
                }
            }
        }
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllUsersByManagerAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = new SqlCommand(GetManagerEmployeesQuery, connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync();
                using(var reader = await command.ExecuteReaderAsync())
                {
                    var employees = new List<EmployeeDto>();
                    while (await reader.ReadAsync())
                    {
                        employees.Add(new EmployeeDto
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ManagerID = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                            Enable = reader.GetBoolean(3)
                        });
                    }
                    return employees;
                }
            }
        }
    }

    public async Task EnableEmployeeAsync(int id, bool enable)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = new SqlCommand(EnableEmployeeQuery, connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                command.Parameters.Add("@Enable", SqlDbType.Bit).Value = enable;

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

}