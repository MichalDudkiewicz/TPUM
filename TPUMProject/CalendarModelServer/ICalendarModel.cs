using System;
using System.Collections.ObjectModel;

namespace CalendarModelServer
{
    public interface ICalendarModel
    {
        public ObservableCollection<IAvailability> availabilities();

        public void setActiveEmployeeId(int id);

        public int getActiveEmployeeId();

        public void AddActiveEmployeeAvailability(DateTime startTime, DateTime endTime);

        public void RemoveActiveEmployeeAvailability(Guid id);
    }
}
