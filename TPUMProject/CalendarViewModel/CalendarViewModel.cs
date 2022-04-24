using System;
using CalendarModel;
using CalendarData;
using System.Collections.ObjectModel;

namespace CalendarViewModel
{
    class CalendarViewModel
    {
        private CalendarModel.CalendarModel calendarModel;

        static void Main(string[] args)
        {
            
        }

        CalendarViewModel(CalendarModel.CalendarModel newCalendarModel)
        {
            calendarModel = newCalendarModel;
        }

        public int ActiveEmployeeId
        {
            get { return calendarModel.ActiveEmployeeId; }
            set { calendarModel.ActiveEmployeeId = value; }
        }

        public ObservableCollection<Availability> ActiveEmployeeAvailabilities
        {
            get { return calendarModel.ActiveEmployeeAvailabilities; }
            set { calendarModel.ActiveEmployeeAvailabilities = value; }
        }

    }
}
