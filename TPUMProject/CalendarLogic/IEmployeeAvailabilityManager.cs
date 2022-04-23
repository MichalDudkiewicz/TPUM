using System;

namespace CalendarLogic
{
    public interface IEmployeeAvailabilityManager
    {
        public void addAvailability(int employeeId, int startTime, int endTime);
        public void addXorAvailability(int employeeId, int startTime, int endTime, int groupId);
        public void removeAvailability(int employeeId, Guid id);
    }
}
