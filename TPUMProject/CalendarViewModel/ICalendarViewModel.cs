using System;
using System.Collections.ObjectModel;

namespace CalendarViewModel
{
    public interface ICalendarViewModel
    {
        public int ActiveEmployeeId
        {
            get;
            set;
        }

        public ObservableCollection<CalendarViewModel.IAvailability> ActiveEmployeeAvailabilities
        {
            get;
        }

        public DateTime MarkedAvailability
        {
            get;
            set;
        }

    }
}
