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
        public EmployeeRepository _employeeRepository;
        private ObservableCollection<IAvailability> availabilities;
        private int activeEmployeeId = 0;
        OuterActionSimulation simulation;

        public ObservableCollection<IAvailability> getAvailabilities()
        {
            return availabilities;
        }

        public void setActiveEmployeeId(int activeEmployeeId)
        {
            _employeeRepository.GetById(activeEmployeeId).Availabilities.CollectionChanged -= onAvailabilitesChange;
            this.activeEmployeeId = activeEmployeeId;
            _employeeRepository.GetById(activeEmployeeId).Availabilities.CollectionChanged += onAvailabilitesChange;
            var newAvailabilities = _employeeRepository.GetById(activeEmployeeId).Availabilities.ToList();
            List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarData.Availability, IAvailability>(Convert));
            availabilities = new ObservableCollection<IAvailability>(newLogicAvailabilities);
        }

        public int getActiveEmployeeId()
        {
            return activeEmployeeId;
        }

        public static IAvailability Convert(CalendarData.Availability a)
        {
            return new Availability(a);
        }

        private void onAvailabilitesChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newAvailabilities = _employeeRepository.GetById(activeEmployeeId).Availabilities.ToList();
            List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarData.Availability, IAvailability>(Convert));

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

        public EmployeeAvailabilityManager(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            availabilities = new ObservableCollection<IAvailability>();
            _employeeRepository.GetById(activeEmployeeId).Availabilities.CollectionChanged += onAvailabilitesChange;

            simulation = new OuterActionSimulation(this, 3.0f);
        }

        public EmployeeAvailabilityManager()
        {
            _employeeRepository = new EmployeeRepository();
            availabilities = new ObservableCollection<IAvailability>();
            _employeeRepository.GetById(activeEmployeeId).Availabilities.CollectionChanged += onAvailabilitesChange;

            simulation = new OuterActionSimulation(this, 10.0f);
            simulation.Start();
        }

        public void addAvailability(DateTime startTime, DateTime endTime)
        {
            _employeeRepository.GetById(activeEmployeeId).addAvailability(startTime, endTime);
        }

        public void removeAvailability(Guid id)
        {
            _employeeRepository.GetById(activeEmployeeId).removeAvailability(id);
        }
    }
}
