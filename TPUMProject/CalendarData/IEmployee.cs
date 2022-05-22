using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CalendarData
{
    public interface IEmployee
    {
        ObservableCollection<IAvailability> Availabilities();
        int GetId();
        void SetId(int id);
        void addAvailability(DateTime startTime, DateTime endTime);
        void addAvailability(IAvailability availability);
        void removeAvailability(Guid id);
    }
}
