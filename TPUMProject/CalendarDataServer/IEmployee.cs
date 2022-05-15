using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CalendarDataServer
{
    public interface IEmployee
    {
        ObservableCollection<IAvailability> Availabilities();
        int GetId();
        void addAvailability(DateTime startTime, DateTime endTime);
        void removeAvailability(Guid id);
    }
}
