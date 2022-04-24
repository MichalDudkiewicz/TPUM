using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CalendarData;
using CalendarLogic;

namespace CalendarModel
{
    public class CalendarModel
    {
        public IEmployeeAvailabilityManager _employeeAvailabilityManager;

        public CalendarModel(IEmployeeAvailabilityManager employeeAvailabilityManager)
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
