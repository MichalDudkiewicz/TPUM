using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarData
{
    public class Availability
    {
        DateTime startTime;
        DateTime endTime;
        Guid Id;

        //public Guid Id()
        //{

        //}
        public Availability(DateTime startTime, DateTime endTime)
        {
            Id = Guid.NewGuid();
        }

        public void setStartTime(DateTime time)
        {
            startTime = time;
        }

        public void setEndTime(DateTime time)
        {
            endTime = time;
        }

    }
}
