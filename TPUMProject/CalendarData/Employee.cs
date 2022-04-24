using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CalendarData
{
    public class Employee
    {
        int id;
        private ObservableCollection<Availability> availabilities;
        Dictionary<int, Guid> xorAvailabilitiesGroups;

        public int Id { get => id; set => id = value; }

        public ObservableCollection<Availability> Availabilities
        {
            get { return availabilities; }
            set
            {
                availabilities = value;
            }
        }

        public void addAvailability(DateTime startTime, DateTime endTime)
        {
            Availability availability = new Availability(startTime, endTime);
            availabilities.Add(availability);
        }

        public void addXorAvailability(int startTime, int endTime, int groupId)
        {

        }

        public void removeAvailability(Guid id)
        {
            availabilities.
        }

        //public Availability getAvailability(Guid id)
        //{
            
        //}
    }
}
