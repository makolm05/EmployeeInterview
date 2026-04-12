using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmployeeService.Repositories;

namespace EmployeeService.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeDto> GetEmployeeTreeAsync(int id)
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            var root = employees.FirstOrDefault(e => e.ID == id);

            if (root == null)
                throw new KeyNotFoundException($"Employee with ID {id} not found");

            root.Employees = BuildTreeRecursive(root, employees.ToList());
            return root;
        }

        public async Task<EmployeeDto> GetEmployeeTreeCTEAsync(int id)
        {
            var employees = await _employeeRepository.GetAllEmployeesByManagerAsync(id);

            if (employees.Count() == 0)
            {
                throw new KeyNotFoundException($"Employees by request ID {id} not found");
            }

            return BuildEmployeeTree(employees.ToList());
        }

        public async Task EnableEmployeeAsync(int id, bool enable)
        {
            await _employeeRepository.EnableEmployeeAsync(id, enable);
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
            if (parent.ID == parent.ManagerID)
            {
                return new List<EmployeeDto>();
            }

            parent.Employees = employees.Where(e => e.ManagerID == parent.ID).ToList();
            parent.Employees.ForEach(c => BuildTreeRecursive(c, employees));
            return parent.Employees;
        }
    }
}