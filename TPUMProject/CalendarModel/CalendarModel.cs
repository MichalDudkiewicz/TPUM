using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CalendarData;
using System.Runtime.CompilerServices;
using CalendarLogic;
using System.ComponentModel;

namespace CalendarModel
{
    public class CalendarModel
    {
        public IEmployeeAvailabilityManager _employeeAvailabilityManager;
        public event PropertyChangedEventHandler PropertyChanged;


        public CalendarModel(IEmployeeAvailabilityManager employeeAvailabilityManager)
        {
            _employeeAvailabilityManager = employeeAvailabilityManager;
        }

        static void Main(string[] args)
        {

        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool DeadlineLockValue
        {
            get { return _employeeAvailabilityManager._employeeRepository.DeadlineLock; }
            set { _employeeAvailabilityManager._employeeRepository.DeadlineLock = value; NotifyPropertyChanged(); }
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
