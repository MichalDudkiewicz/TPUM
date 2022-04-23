using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarLogic
{
    class Availability
    {
        DateTime startTime;
        DateTime endTime;
        Guid Id;

        //public Guid Id()
        //{

        //}

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
