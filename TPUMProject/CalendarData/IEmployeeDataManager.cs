using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace CalendarData
{
    public interface IEmployeeDataManager
    {
        public ObservableCollection<IAvailability> Availabilities();

        public int ActiveEmployeeId();

        public abstract Task connect();

        public abstract Task disconnect();

        public void AddAvailability(Guid id, DateTime startTime, DateTime endTime);

        public void removeAvailability(Guid id);
    }
}
