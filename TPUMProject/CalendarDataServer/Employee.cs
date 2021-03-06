using System;
using System.Collections.ObjectModel;
using System.Linq;

#if (DEBUG)
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CalendarDataServer.Test"), InternalsVisibleTo("CalendarLogicServer.Test")]
#endif

namespace CalendarDataServer
{
    internal class Employee : IEmployee
    {
        private int id;
        private ObservableCollection<IAvailability> availabilities;
        private readonly object mutex = new object();

        public Employee(int newId)
        {
            id = newId;
            availabilities = new ObservableCollection<IAvailability>();
        }

        public ObservableCollection<IAvailability> Availabilities()
        {
            lock (mutex)
            {
                return availabilities;
            }
        }

        public void addAvailability(DateTime startTime, DateTime endTime)
        {
            lock (mutex)
            {
                IAvailability availability = new Availability(startTime, endTime);
                Availabilities().Add(availability);
            }
        }

        public void removeAvailability(Guid id)
        {
            lock (mutex)
            {
                Availabilities().Remove(availabilities.Single(a => a.id() == id));
            }
        }

        public int GetId()
        {
            return id;
        }
    }
}
