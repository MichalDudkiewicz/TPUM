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
        private readonly object mutex = new object();

        public Employee(int newId)
        {
            id = newId;
            availabilities = new ObservableCollection<Availability>();
        }

        public int Id { get => id; }

        public ObservableCollection<Availability> Availabilities
        {
            get { 
                lock (mutex)
                {
                    return availabilities;
                }
            }
        }

        public void addAvailability(DateTime startTime, DateTime endTime)
        {
            Availability availability = new Availability(startTime, endTime);
            Availabilities.Add(availability);
        }

        public void removeAvailability(Guid id)
        {
            Availabilities.Remove(availabilities.Single(a => a.Id == id));
        }
    }
}
