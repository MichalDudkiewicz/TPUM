using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CalendarData;

namespace CalendarLogic
{
    public class IEmployeeAvailabilityManager
    {
        public EmployeeRepository _employeeRepository;
        private ObservableCollection<Availability> availabilities;
        private int activeEmployeeId = 0;
        OuterActionSimulation simulation;

        public ObservableCollection<Availability> Availabilities
        {
            get
            {
                return availabilities;
            }
        }

        public int ActiveEmployeeId
        {
            get
            {
                return activeEmployeeId;
            }
            set {
                _employeeRepository.GetById(activeEmployeeId).Availabilities.CollectionChanged -= onAvailabilitesChange;
                activeEmployeeId = value;
                _employeeRepository.GetById(activeEmployeeId).Availabilities.CollectionChanged += onAvailabilitesChange;
                var newAvailabilities = _employeeRepository.GetById(activeEmployeeId).Availabilities.ToList();
                List<Availability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarData.Availability, Availability>(Convert));
                availabilities = new ObservableCollection<Availability>(newLogicAvailabilities);
            }
        }

        public static Availability Convert(CalendarData.Availability a)
        {
            return new Availability(a);
        }

        private void onAvailabilitesChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newAvailabilities = _employeeRepository.GetById(activeEmployeeId).Availabilities.ToList();
            List<Availability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarData.Availability, Availability>(Convert));

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    availabilities.Add(Convert((CalendarData.Availability)item));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                availabilities.Clear();
            }
        }

        public bool DeadlineLock
        {
            get { return _employeeRepository.DeadlineLock; }
            set { _employeeRepository.DeadlineLock = value; }
        }

        public IEmployeeAvailabilityManager(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            availabilities = new ObservableCollection<Availability>();
            _employeeRepository.GetById(activeEmployeeId).Availabilities.CollectionChanged += onAvailabilitesChange;

            simulation = new OuterActionSimulation(this, 3.0f);
        }

        public IEmployeeAvailabilityManager()
        {
            _employeeRepository = new EmployeeRepository();
            availabilities = new ObservableCollection<Availability>();
            _employeeRepository.GetById(activeEmployeeId).Availabilities.CollectionChanged += onAvailabilitesChange;

            simulation = new OuterActionSimulation(this, 10.0f);
            simulation.Start();
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
