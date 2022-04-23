using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarLogic
{
    public class Employee
    {
        int Id;
        List<Availability> availabilities;
        Dictionary<int, Guid> xorAvailabilitiesGroups;

        private readonly IEmployeeAvailabilityManager _employeeAvailabilityManager;

        public Employee(IEmployeeAvailabilityManager employeeAvailabilityManager) => 
            _employeeAvailabilityManager = employeeAvailabilityManager;

        public void addAvailability(int startTime, int endTime)
        {
            
        }

        public void addXorAvailability(int startTime, int endTime, int groupId)
        {

        }

        public void removeAvailability(Guid id)
        {

        }

        //public Availability getAvailability(Guid id)
        //{
            
        //}
    }
}
