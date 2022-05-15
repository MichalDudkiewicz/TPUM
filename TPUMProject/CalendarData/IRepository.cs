﻿using System.Collections.Generic;
using System;

namespace CalendarData
{
    public abstract class IRepository<Type>
    {
        protected Dictionary<int, Type> repositoryEntities;

        public Type GetById(int id)
        {
            return repositoryEntities[id];
        }
        public abstract void defaultInitialize();

        public abstract void AddAvailability(int employeeId, DateTime startTime, DateTime endTime);
    }
}
