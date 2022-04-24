using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CalendarData
{
    public class EmployeeRepository : Repository<Employee>
    {
        public EmployeeRepository()
        {
            repositoryEntities = new Dictionary<int, Employee>();
            Employee employee = new Employee(0);
            repositoryEntities.Add(0,employee);
        }

        public EmployeeRepository(List<Employee> employees)
        {
            repositoryEntities = employees.ToDictionary(keySelector: e => e.Id, elementSelector: e => e);
        }
    }
}
