using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarData
{
    public class Availability
    {
        private DateTime startTime;
        private DateTime endTime;
        private Guid id;

        
        public Guid Id
        {
            get { return id; }
        }

        public Availability(DateTime startTime, DateTime endTime)
        {
            id = Guid.NewGuid();
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
