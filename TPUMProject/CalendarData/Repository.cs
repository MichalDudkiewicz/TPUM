using System;
using System.Collections.Generic;

namespace CalendarData
{
    abstract class Repository<Employee> : IRepository<Employee>
    {
        private List<Type> entities;
        public abstract Employee getById(int id);
    }
}
