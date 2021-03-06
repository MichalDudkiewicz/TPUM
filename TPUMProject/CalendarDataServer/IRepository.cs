using System.Collections.Generic;

namespace CalendarDataServer
{
    public abstract class IRepository<Type>
    {
        protected Dictionary<int, Type> repositoryEntities;

        public Type GetById(int id)
        {
            return repositoryEntities[id];
        }
        public abstract void defaultInitialize();
    }
}
