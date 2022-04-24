using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CalendarLogic;

namespace CalendarModel
{
    public class CalendarModel
    {
        private IEmployeeAvailabilityManager _employeeAvailabilityManager;

        CalendarModel(IEmployeeAvailabilityManager employeeAvailabilityManager)
        {
            _employeeAvailabilityManager = employeeAvailabilityManager;
        }

        static void Main(string[] args)
        {

        }

        public int ActiveEmployeeId
        {
            get { return _employeeAvailabilityManager.ActiveEmployeeId;  }
            set { _employeeAvailabilityManager.ActiveEmployeeId = value;  }
        }

        public ObservableCollection<CalendarData.Availability> ActiveEmployeeAvailabilities
        {
            get { return _employeeAvailabilityManager.ActiveEmployeeAvailabilities; }
            set { _employeeAvailabilityManager.ActiveEmployeeAvailabilities = value; }
        }

        public void AddActiveEmployeeAvailability(DateTime startTime, DateTime endTime)
        {
            _employeeAvailabilityManager.addAvailability(startTime, endTime);
        }

        public void RemoveActiveEmployeeAvailability(Guid id)
        {
            _employeeAvailabilityManager.removeAvailability(id);
        }
    }
}
