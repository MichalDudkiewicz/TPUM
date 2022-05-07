using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using CalendarData;

namespace CalendarLogic
{
    public class IEmployeeAvailabilityManager
    {
        public EmployeeRepository _employeeRepository;
        private ObservableCollection<Availability> availabilities;
        private int activeEmployeeId = -1;

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
                if (activeEmployeeId != -1)
                {
                    _employeeRepository.GetById(activeEmployeeId).Availabilities.CollectionChanged -= onAvailabilitesChange;
                }
                activeEmployeeId = value;
                _employeeRepository.GetById(activeEmployeeId).Availabilities.CollectionChanged += onAvailabilitesChange;
                var employeeAvailabilities = _employeeRepository.GetById(activeEmployeeId).Availabilities;
                availabilities = new ObservableCollection<Availability>((System.Collections.Generic.IEnumerable<Availability>)employeeAvailabilities);
            }
        }

        private void onAvailabilitesChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            availabilities = (ObservableCollection<Availability>)e.NewItems;
        }

        public bool DeadlineLock
        {
            get { return _employeeRepository.DeadlineLock; }
            set { _employeeRepository.DeadlineLock = value; }
        }


        public ObservableCollection<CalendarLogic.Availability> ActiveEmployeeAvailabilities
        {
            get { return Availabilities; }
        }

        public IEmployeeAvailabilityManager(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IEmployeeAvailabilityManager()
        {
            _employeeRepository = new EmployeeRepository();
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
