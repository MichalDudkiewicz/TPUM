using System;
using System.Collections.Generic;

namespace CalendarData
{
    public interface IRepository<Type>
    {
        public Type getById(int id);
    }
}
