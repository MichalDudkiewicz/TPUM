using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CalendarLogic
{
    public interface IEmployeeAvailabilityManager
    {
        public void setActiveEmployeeId(int id);

        public int getActiveEmployeeId();

        public void AddAvailability(Guid id, DateTime startTime, DateTime endTime);

        public void removeAvailability(Guid id);

        public ObservableCollection<IAvailability> getAvailabilities();
    }
}
