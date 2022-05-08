using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
