using System.Threading.Tasks;
using EmployeeService.Services;

namespace EmployeeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EmployeeService.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class EmployeeWCFService : IEmployeeWCFService
    {

        IEmployeeService _employeeService;

        public EmployeeWCFService()
        {
            _employeeService = new EmployeeService.Services.EmployeeService(new EmployeeRepository());
        }

        public async Task<EmployeeDto> GetEmployeeById(int id)
        {
            return await _employeeService.GetEmployeeTreeAsync(id);
        }

        public async Task<EmployeeDto> GetEmployeeCTEById(int id)
        {
            return await _employeeService.GetEmployeeTreeCTEAsync(id);
        }

        public async Task EnableEmployee(int id, bool enable)
        {
            await _employeeService.EnableEmployeeAsync(id, enable);
        }
    }
      
}