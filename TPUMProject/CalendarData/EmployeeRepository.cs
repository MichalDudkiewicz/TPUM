using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarData
{
    public class EmployeeRepository : IRepository<IEmployee>
    {
        private bool deadlineLock;
        public event Action<bool> onDeadlineLockChange;
        private readonly object mutex = new object();

        public EmployeeRepository()
        {
            repositoryEntities = new Dictionary<int, IEmployee>();
        }

        public override void defaultInitialize()
        {
            IEmployee employee = new Employee(0);
            repositoryEntities.Add(0, employee);

            IEmployee employee2 = new Employee(1);
            repositoryEntities.Add(1, employee2);
        }

        public bool DeadlineLock
        {
            get {
                lock (mutex)
                {
                    return deadlineLock;
                }
            }
            set { 
                lock(mutex)
                {
                    deadlineLock = value;
                    onDeadlineLockChange?.Invoke(value);
                }
            }
        }

        public EmployeeRepository(List<IEmployee> employees)
        {
            deadlineLock = false;
            repositoryEntities = employees.ToDictionary(keySelector: e => e.GetId(), elementSelector: e => e);
        }
    }
}
