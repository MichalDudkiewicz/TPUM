using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CalendarLogic
{
    public class Availability : IAvailability
    {
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public Guid id { get; set; }

        public Availability(CalendarData.Availability availability)
        {
            startTime = availability.StartTime;
            endTime = availability.EndTime;
            id = availability.Id;
        }

        public Availability(Guid _id, DateTime _startTime, DateTime _endTime)
        {
            startTime = _startTime;
            endTime = _endTime;
            id = _id;
        }

        public static implicit operator Availability(CalendarData.Availability a) => new Availability(a.Id, a.StartTime, a.EndTime);
        public static explicit operator CalendarData.Availability(Availability a) => new CalendarData.Availability(a.startTime, a.endTime);

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
