using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarViewModel
{
    public class Availability
    {
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public Guid id { get; set; }

        public Availability(CalendarModel.Availability availability)
        {
            startTime = availability.startTime;
            endTime = availability.endTime;
            id = availability.id;
        }

        public Availability(Guid _id, DateTime _startTime, DateTime _endTime)
        {
            startTime = _startTime;
            endTime = _endTime;
            id = _id;
        }

        public static implicit operator Availability(CalendarModel.Availability a) => new Availability(a.id, a.startTime, a.endTime);

        public bool Equals(Availability other)
        {
            return id == other.id;
        }
    }
}
