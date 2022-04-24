using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CalendarData;

namespace CalendarLogic
{
    public class IEmployeeAvailabilityManager
    {
        public EmployeeRepository _employeeRepository;

        public int ActiveEmployeeId
        {
            get; set;
        }


        public ObservableCollection<Availability> ActiveEmployeeAvailabilities
        {
            get { return _employeeRepository.GetById(ActiveEmployeeId).Availabilities; }
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
