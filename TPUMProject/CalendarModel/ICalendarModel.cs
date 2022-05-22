using System;
using System.Collections.ObjectModel;

namespace CalendarModel
{
    public interface ICalendarModel
    {
        public ObservableCollection<IAvailability> availabilities();

        public void setActiveEmployeeId(int id);

        public int getActiveEmployeeId();

        public void AddActiveEmployeeAvailability(Guid id, DateTime startTime, DateTime endTime);

        public void RemoveActiveEmployeeAvailability(Guid id);
    }
}
