using System;
using System.Collections.Generic;
using System.Text;
using CalendarLogic;

namespace CalendarData
{
    class EmployeeRepository : Repository<Employee>
    {
        public override Employee getById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
