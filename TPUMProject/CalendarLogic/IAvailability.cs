﻿using System;

namespace CalendarLogic
{
    public interface IAvailability : IEquatable<IAvailability>
    {
        public DateTime startTime();
        public DateTime endTime();
        public Guid id();
    }
}
