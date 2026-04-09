namespace EmployeeService.Repositories
{
    public interface IEmployeeRepository
    {
        EmployeeDto GetEmployeeTree(int id);
        EmployeeDto GetEmployeeTreeCTE(int id);
        void EnableEmployee(int id, bool enable);
    }
}
