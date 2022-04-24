using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CalendarData
{
    public class EmployeeRepository : Repository<Employee>
    {
        private bool deadlineLock;
        public event Action<bool> onDeadlineLockChange;
        private readonly object mutex = new object();

        public EmployeeRepository()
        {
            repositoryEntities = new Dictionary<int, Employee>();
            Employee employee = new Employee(0);
            repositoryEntities.Add(0, employee);
        }

        public bool DeadlineLock
        {
            get { return deadlineLock; }
            set { 
                lock(mutex)
                {
                    deadlineLock = value;
                    onDeadlineLockChange?.Invoke(value);
                }
            }
        }

        public EmployeeRepository(List<Employee> employees)
        {
            deadlineLock = false;
            repositoryEntities = employees.ToDictionary(keySelector: e => e.Id, elementSelector: e => e);
        }
    }
}
