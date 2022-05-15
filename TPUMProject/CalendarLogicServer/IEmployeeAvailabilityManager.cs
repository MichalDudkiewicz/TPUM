using System;
using System.Collections.ObjectModel;

namespace CalendarLogicServer
{
    public interface IEmployeeAvailabilityManager
    {
        public void setActiveEmployeeId(int id);

        public int getActiveEmployeeId();

        public void addAvailability(DateTime startTime, DateTime endTime);

        public void removeAvailability(Guid id);

        public ObservableCollection<IAvailability> getAvailabilities();
    }
}
