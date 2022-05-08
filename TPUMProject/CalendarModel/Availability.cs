using System;

namespace CalendarModel
{
    internal class Availability : IAvailability
    {
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public Guid id { get; set; }

        public Availability(CalendarLogic.IAvailability availability)
        {
            startTime = availability.startTime();
            endTime = availability.endTime();
            id = availability.id();
        }

        public bool Equals(Availability other)
        {
            return id == other.id;
        }

        DateTime IAvailability.startTime()
        {
            return startTime;
        }

        DateTime IAvailability.endTime()
        {
            return endTime;
        }

        Guid IAvailability.id()
        {
            return id;
        }

        public bool Equals(IAvailability other)
        {
            return id == other.id();
        }
    }
}
