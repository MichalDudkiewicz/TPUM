using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CalendarData
{
    public class EmployeeRepository : Repository<Employee>
    {
        public EmployeeRepository(List<Employee> employees)
        {
            repositoryEntities = employees.ToDictionary(keySelector: e => e.Id, elementSelector: e => e);
        }
    }
}
