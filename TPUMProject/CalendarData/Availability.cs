using System;

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
        
        public DateTime StartTime
        {
            get { return startTime; }
        }
        
        public DateTime EndTime
        {
            get { return endTime; }
        }

        public Availability(DateTime starttime, DateTime endtime)
        {
            id = Guid.NewGuid();
            startTime = starttime;
            endTime = endtime;
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
