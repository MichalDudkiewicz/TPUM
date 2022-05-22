using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;
using CalendarData;

namespace CalendarLogic
{
    public class EmployeeAvailabilityManager : IEmployeeAvailabilityManager
    {
        private ObservableCollection<IAvailability> availabilities;
        private IEmployeeDataManager _owningEmployeeManager;

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
                _owningEmployeeManager.Availabilities().CollectionChanged -= onAvailabilitesChange;
                var newAvailabilities = _owningEmployeeManager.Availabilities().ToList();
                List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarData.IAvailability, IAvailability>(Convert));
                availabilities = new ObservableCollection<IAvailability>(newLogicAvailabilities);
                _owningEmployeeManager.Availabilities().CollectionChanged += onAvailabilitesChange;
            }
        }

        public int getActiveEmployeeId()
        {
            lock (_dataLock)
            {
                return _owningEmployeeManager.ActiveEmployeeId();
            }
        }

        public static IAvailability Convert(CalendarData.IAvailability a)
        {
            return new Availability(a);
        }

        private void onAvailabilitesChange(object sender, NotifyCollectionChangedEventArgs e)
        {
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

        public EmployeeAvailabilityManager(int owningEmployeeId)
        {
            _owningEmployeeManager = new EmployeeDataManager(owningEmployeeId);
            _owningEmployeeManager.connect();
           
            var newAvailabilities = _owningEmployeeManager.Availabilities().ToList();
            List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarData.IAvailability, IAvailability>(Convert));
            availabilities = new ObservableCollection<IAvailability>(newLogicAvailabilities);
            _owningEmployeeManager.Availabilities().CollectionChanged += onAvailabilitesChange;

            BindingOperations.EnableCollectionSynchronization(availabilities, _dataLock);
        }

        public void AddAvailability(Guid id, DateTime startTime, DateTime endTime)
        {
            lock (_dataLock)
            {
                _owningEmployeeManager.AddAvailability(id, startTime, endTime);
            }
        }

        public void removeAvailability(Guid id)
        {
            lock (_dataLock)
            {
                _owningEmployeeManager.removeAvailability(id);
            }
        }
    }
}
