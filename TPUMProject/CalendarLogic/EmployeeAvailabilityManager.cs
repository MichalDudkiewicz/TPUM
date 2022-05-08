using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CalendarData;

namespace CalendarLogic
{
    public class EmployeeAvailabilityManager : IEmployeeAvailabilityManager
    {
        public IRepository<IEmployee> _employeeRepository;
        private ObservableCollection<IAvailability> availabilities;
        private int activeEmployeeId = 0;
        OuterActionSimulation simulation;
        private readonly object _dataLock = new object();

        public ObservableCollection<IAvailability> getAvailabilities()
        {
            lock (_dataLock)
            {
                return availabilities;
            }
        }

        public void setActiveEmployeeId(int activeEmployeeId)
        {
            lock (_dataLock)
            {
                _employeeRepository.GetById(activeEmployeeId).Availabilities().CollectionChanged -= onAvailabilitesChange;
                this.activeEmployeeId = activeEmployeeId;
                _employeeRepository.GetById(activeEmployeeId).Availabilities().CollectionChanged += onAvailabilitesChange;
                var newAvailabilities = _employeeRepository.GetById(activeEmployeeId).Availabilities().ToList();
                List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarData.IAvailability, IAvailability>(Convert));
                availabilities = new ObservableCollection<IAvailability>(newLogicAvailabilities);
            }
        }

        public int getActiveEmployeeId()
        {
            lock (_dataLock)
            {
                return activeEmployeeId;
            }
        }

        public static IAvailability Convert(CalendarData.IAvailability a)
        {
            return new Availability(a);
        }

        private void onAvailabilitesChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newAvailabilities = _employeeRepository.GetById(activeEmployeeId).Availabilities().ToList();
            List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarData.IAvailability, IAvailability>(Convert));

            lock (_dataLock)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in e.NewItems)
                    {
                        availabilities.Add(Convert((CalendarData.IAvailability)item));
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    availabilities.Clear();
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (var item in e.OldItems)
                    {
                        availabilities.Remove(Convert((CalendarData.IAvailability)item));
                    }
                }
            }
        }

        public EmployeeAvailabilityManager(IRepository<IEmployee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
            availabilities = new ObservableCollection<IAvailability>();
            _employeeRepository.GetById(activeEmployeeId).Availabilities().CollectionChanged += onAvailabilitesChange;

            simulation = new OuterActionSimulation(this, 3.0f);
        }

        public EmployeeAvailabilityManager()
        {
            _employeeRepository = new EmployeeRepository();
            _employeeRepository.defaultInitialize();
            availabilities = new ObservableCollection<IAvailability>();
            _employeeRepository.GetById(activeEmployeeId).Availabilities().CollectionChanged += onAvailabilitesChange;

            simulation = new OuterActionSimulation(this, 10.0f);
            simulation.Start();
        }

        public void addAvailability(DateTime startTime, DateTime endTime)
        {
            lock (_dataLock)
            {
                _employeeRepository.GetById(activeEmployeeId).addAvailability(startTime, endTime);
            }
        }

        public void removeAvailability(Guid id)
        {
            lock (_dataLock)
            {
                _employeeRepository.GetById(activeEmployeeId).removeAvailability(id);
            }
        }
    }
}
