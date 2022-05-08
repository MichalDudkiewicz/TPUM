using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarModel
{
    public class Availability : IEquatable<Availability>
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
    }
}
