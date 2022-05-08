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
    public class CalendarModel : ICalendarModel
    {
        public IEmployeeAvailabilityManager _employeeAvailabilityManager;
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<IAvailability> _availabilites;

        private void onAvailabilitesChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    _availabilites.Add(Convert((CalendarLogic.IAvailability)item));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                _availabilites.Clear();
            }
        }

        public static IAvailability Convert(CalendarLogic.IAvailability a)
        {
            return new Availability(a);
        }

        public CalendarModel(IEmployeeAvailabilityManager manager)
        {
            _employeeAvailabilityManager = manager;
            _availabilites = new ObservableCollection<IAvailability>();
            _employeeAvailabilityManager.getAvailabilities().CollectionChanged += onAvailabilitesChange;
        }

        public CalendarModel()
        {
            _employeeAvailabilityManager = new EmployeeAvailabilityManager();
            _availabilites = new ObservableCollection<IAvailability>();
            _employeeAvailabilityManager.getAvailabilities().CollectionChanged += onAvailabilitesChange;
        }

        static void Main(string[] args)
        {

        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int ActiveEmployeeId
        {
            get { return _employeeAvailabilityManager.getActiveEmployeeId();  }
            set {
                _employeeAvailabilityManager.getAvailabilities().Clear();
                _employeeAvailabilityManager.getAvailabilities().CollectionChanged -= onAvailabilitesChange;
                _employeeAvailabilityManager.setActiveEmployeeId(value);
               
                List<CalendarLogic.IAvailability> newAvailabilities = _employeeAvailabilityManager.getAvailabilities().ToList();
                List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarLogic.IAvailability, IAvailability>(Convert));
                _availabilites = new ObservableCollection<IAvailability>(newLogicAvailabilities);
                _employeeAvailabilityManager.getAvailabilities().CollectionChanged += onAvailabilitesChange;
            }
        }

        public void AddActiveEmployeeAvailability(DateTime startTime, DateTime endTime)
        {
            _employeeAvailabilityManager.addAvailability(startTime, endTime);
        }

        public void RemoveActiveEmployeeAvailability(Guid id)
        {
            _employeeAvailabilityManager.removeAvailability(id);
        }

        public ObservableCollection<IAvailability> availabilities()
        {
            return _availabilites;
        }

        public void setActiveEmployeeId(int id)
        {
            ActiveEmployeeId = id;
        }

        public int getActiveEmployeeId()
        {
            return ActiveEmployeeId;
        }
    }
}
