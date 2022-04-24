using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CalendarData
{
    public class Employee
    {
        private int id;
        private ObservableCollection<Availability> availabilities;

        public Employee()
        {
            availabilities = new ObservableCollection<Availability>();
        }

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

        public void removeAvailability(Guid id)
        {
            availabilities.Remove(availabilities.Single(a => a.Id == id));
        }
    }
}
