using System;

namespace CalendarModel
{
    public interface IAvailability : IEquatable<IAvailability>
    {
        public DateTime startTime();
        public DateTime endTime();
        public Guid id();
    }
}
