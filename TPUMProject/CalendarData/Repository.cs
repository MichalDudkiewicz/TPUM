using System;
using System.Collections.Generic;

namespace CalendarData
{
    public abstract class Repository<Type>
    {
        protected Dictionary<int, Type> repositoryEntities;

        public Type GetById(int id)
        {
            return repositoryEntities[id];
        }
    }
}
