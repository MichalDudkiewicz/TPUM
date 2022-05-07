using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CalendarData;

namespace CalendarLogic
{
    public class IEmployeeAvailabilityManager
    {
        public EmployeeRepository _employeeRepository;
        private ObservableCollection<Availability> availabilities;

        public ObservableCollection<Availability> Availabilities
        {
            get
            {
                return availabilities;
            }
        }

        public int ActiveEmployeeId
        {
            get; set;
        }

        public bool DeadlineLock
        {
            get { return _employeeRepository.DeadlineLock; }
            set { _employeeRepository.DeadlineLock = value; }
        }


        public ObservableCollection<CalendarLogic.Availability> ActiveEmployeeAvailabilities
        {
            get { return Availabilities; }
        }

        public IEmployeeAvailabilityManager(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IEmployeeAvailabilityManager()
        {
            _employeeRepository = new EmployeeRepository();
        }
        
        public void addAvailability(DateTime startTime, DateTime endTime)
        {
            _employeeRepository.GetById(ActiveEmployeeId).addAvailability(startTime, endTime);
        }

        public void removeAvailability(Guid id)
        {
            _employeeRepository.GetById(ActiveEmployeeId).removeAvailability(id);
        }
    }
}
