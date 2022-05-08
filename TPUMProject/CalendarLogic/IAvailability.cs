using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarLogic
{
    public interface IAvailability : IEquatable<IAvailability>
    {
        public DateTime startTime();
        public DateTime endTime();
        public Guid id();
    }
}
