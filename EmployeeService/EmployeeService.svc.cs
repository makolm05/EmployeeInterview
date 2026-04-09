using System.Threading.Tasks;
using EmployeeService.Repositories;

namespace EmployeeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EmployeeService.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class EmployeeService : IEmployeeService
    {

        IEmployeeRepository _repository = new EmployeeRepository();

        public async Task<EmployeeDto> GetEmployeeById(int id)
        {
            return await _repository.GetEmployeeTree(id);
        }

        public async Task<EmployeeDto> GetEmployeeCTEById(int id)
        {
            return await _repository.GetEmployeeTreeCTE(id);
        }

        public async Task EnableEmployee(int id, bool enable)
        {
            await _repository.EnableEmployee(id, enable);
        }
    }
      
}