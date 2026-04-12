using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmployeeService.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeDto>> GetAllUsersAsync();
        Task<IEnumerable<EmployeeDto>> GetAllUsersByManagerAsync(int id);
        Task EnableEmployeeAsync(int id, bool enable);
    }
}
