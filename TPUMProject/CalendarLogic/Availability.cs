using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarLogic
{
    public class Availability
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

        public bool Equals(Availability other)
        {
            return id == other.id;
        }
    }
}
