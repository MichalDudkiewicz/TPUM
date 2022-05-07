using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using CalendarLogic;
using System.ComponentModel;

namespace CalendarModel
{
    public class CalendarModel
    {
        public IEmployeeAvailabilityManager _employeeAvailabilityManager;
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Availability> _availabilites;

        public ObservableCollection<Availability> Availabilities
        {
            get
            {
                return _availabilites;
            }
        }

        public CalendarModel()
        {
            _employeeAvailabilityManager = new IEmployeeAvailabilityManager();

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
            get { return _employeeAvailabilityManager.DeadlineLock; }
            set { _employeeAvailabilityManager.DeadlineLock = value; NotifyPropertyChanged(); }
        }
        public int ActiveEmployeeId
        {
            get { return _employeeAvailabilityManager.ActiveEmployeeId;  }
            set { _employeeAvailabilityManager.ActiveEmployeeId = value;  }
        }

        public ObservableCollection<Availability> ActiveEmployeeAvailabilities
        {
            get { return Availabilities; }
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
