using System;
using System.Collections.ObjectModel;
using CalendarData;

namespace CalendarLogic
{
    public class IEmployeeAvailabilityManager
    {
        private Repository<Employee> iRepository;
        public int ActiveEmployeeId
        {
            get; set;
        }

        public ObservableCollection<Availability> ActiveEmployeeAvailabilities
        {
            get { return iRepository.GetById(ActiveEmployeeId).Availabilities; }
        }

        public IEmployeeAvailabilityManager(Repository<Employee> employeeRepository)
        {
            iRepository = employeeRepository;
        }

        public IEmployeeAvailabilityManager()
        {
            iRepository = new EmployeeRepository();
        }
        
        public void addAvailability(DateTime startTime, DateTime endTime)
        {
            iRepository.GetById(ActiveEmployeeId).addAvailability(startTime, endTime);
        }

        public void removeAvailability(Guid id)
        {
            iRepository.GetById(ActiveEmployeeId).removeAvailability(id);
        }
    }
}
