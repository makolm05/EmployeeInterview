using System.Threading.Tasks;

namespace EmployeeService.Repositories
{
    public interface IEmployeeRepository
    {
        Task<EmployeeDto> GetEmployeeTree(int id);
        Task<EmployeeDto> GetEmployeeTreeCTE(int id);
        Task EnableEmployee(int id, bool enable);
    }
}
