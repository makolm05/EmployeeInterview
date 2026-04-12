using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmployeeService.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesByManagerAsync(int id);
        Task EnableEmployeeAsync(int id, bool enable);
    }
}
