using System.Threading.Tasks;

namespace EmployeeService.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> GetEmployeeTreeAsync(int id);
        Task<EmployeeDto> GetEmployeeTreeCTEAsync(int id);
        Task EnableEmployeeAsync(int id, bool enable);
    }
}
