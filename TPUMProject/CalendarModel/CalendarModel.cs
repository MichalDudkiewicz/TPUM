using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using CalendarLogic;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;

namespace CalendarModel
{
    public class CalendarModel
    {
        public IEmployeeAvailabilityManager _employeeAvailabilityManager;
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Availability> _availabilites;

        public ObservableCollection<Availability> Availabilities
        {
            get
            {
                return _availabilites;
            }
        }

        private void onAvailabilitesChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<CalendarLogic.Availability> senderCollection = sender as ObservableCollection<CalendarLogic.Availability>;
            var newAvailabilities = senderCollection.ToList();
            List<Availability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarLogic.Availability, Availability>(Convert));

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    _availabilites.Add(Convert((CalendarLogic.Availability)item));
                }
            }
        }

        public static Availability Convert(CalendarLogic.Availability a)
        {
            return new Availability(a);
        }

        public CalendarModel()
        {
            _employeeAvailabilityManager = new IEmployeeAvailabilityManager();
            _availabilites = new ObservableCollection<Availability>();
            _employeeAvailabilityManager.Availabilities.CollectionChanged += onAvailabilitesChange;
        }

        static void Main(string[] args)
        {

        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool DeadlineLockValue
        {
            get { return _employeeAvailabilityManager.DeadlineLock; }
            set { _employeeAvailabilityManager.DeadlineLock = value; NotifyPropertyChanged(); }
        }
        public int ActiveEmployeeId
        {
            get { return _employeeAvailabilityManager.ActiveEmployeeId;  }
            set {
                _employeeAvailabilityManager.Availabilities.CollectionChanged -= onAvailabilitesChange;
                _employeeAvailabilityManager.ActiveEmployeeId = value;
                _employeeAvailabilityManager.Availabilities.CollectionChanged += onAvailabilitesChange;
            }
        }

        public ObservableCollection<Availability> ActiveEmployeeAvailabilities
        {
            get { return Availabilities; }
        }

        public void AddActiveEmployeeAvailability(DateTime startTime, DateTime endTime)
        {
            _employeeAvailabilityManager.addAvailability(startTime, endTime);
        }

        public void RemoveActiveEmployeeAvailability(Guid id)
        {
            _employeeAvailabilityManager.removeAvailability(id);
        }
    }
}
