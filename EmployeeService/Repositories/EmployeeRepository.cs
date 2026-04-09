using System;
using System.Data;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmployeeService.Repositories;

public class EmployeeRepository : IEmployeeRepository
{

    private readonly string _connectionString;

    public EmployeeRepository()
    {
        _connectionString = ConfigurationManager.ConnectionStrings["Employee"].ConnectionString;
    }

    public async Task<EmployeeDto> GetEmployeeTree(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = new SqlCommand("SELECT ID, Name, ManagerID, Enable FROM Employee", connection))
            {
                await connection.OpenAsync();

                var employees = new List<EmployeeDto>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        employees.Add(new EmployeeDto
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ManagerID = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                            Enable = reader.GetBoolean(3)
                        });
                    }
                }

                var root = employees.FirstOrDefault(e => e.ID == id);

                if (root == null)
                    throw new Exception($"Employee with ID {id} not found");

                root.Employees = BuildTreeRecursive(root, employees);
                return root;
            }
        }
    }

    public async Task<EmployeeDto> GetEmployeeTreeCTE(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var existsRequest = @"
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

            using (var command = new SqlCommand(existsRequest, connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                await connection.OpenAsync();
                using(var reader = await command.ExecuteReaderAsync())
                {

                    if(!reader.HasRows)
                    {
                        throw new Exception($"Employee with ID {id} not found");
                    }

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

                    return BuildEmployeeTree(employees);
                }
            }
        }

    }

    public async Task EnableEmployee(int id, bool enable)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = @"
            UPDATE Employee
            SET Enable = @Enable
            WHERE ID = @ID";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                command.Parameters.Add("@Enable", SqlDbType.Bit).Value = enable;

                await connection.OpenAsync();

                var rows = await command.ExecuteNonQueryAsync();

                if (rows == 0)
                    throw new Exception($"Employee with ID {id} not found");
            }
        }
    }

    private EmployeeDto BuildEmployeeTree(List<EmployeeDto> employees)
    {
        var dict = employees.ToDictionary(e => e.ID);

        EmployeeDto root = null;

        foreach (var emp in employees)
        {
            if (emp.ManagerID == null || !dict.ContainsKey(emp.ManagerID.Value))
            {
                root = emp;
            }
            else
            {
                dict[emp.ManagerID.Value].Employees.Add(emp);
            }
        }

        return root;
    }

    private List<EmployeeDto> BuildTreeRecursive(EmployeeDto parent, List<EmployeeDto> employees)
    {
        if(parent.ID == parent.ManagerID)
        {
            return new List<EmployeeDto>();
        }
        
        parent.Employees = employees.Where(e => e.ManagerID == parent.ID).ToList();
        parent.Employees.ForEach(c => BuildTreeRecursive(c, employees));
        return parent.Employees;
    }

}