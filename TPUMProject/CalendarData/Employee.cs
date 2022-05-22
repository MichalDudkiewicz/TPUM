using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

#if (DEBUG)
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CalendarData.Test"), InternalsVisibleTo("CalendarLogic.Test")]
#endif

namespace CalendarData
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

            BindingOperations.EnableCollectionSynchronization(availabilities, mutex);
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
                availabilities.Add(availability);
            }
        }
        public void addAvailability(IAvailability availability)
        {
            lock (mutex)
            {
                availabilities.Add(availability);
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

        public void SetId(int id)
        {
            this.id = id;
        }
    }

    public class EmployeeMaker
    {
        public IEmployee CreateEmployee(int id)
        {
            IEmployee newEmployee = new Employee(id);
            return newEmployee;
        }
    }
}
