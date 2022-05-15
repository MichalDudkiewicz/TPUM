using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Data;

namespace CalendarViewModel
{
    public class ViewModel : ICalendarViewModel, INotifyPropertyChanged
    {
        private CalendarModel.ICalendarModel calendarModel;
        private DateTime currentAvailability;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly object mutex = new object();

        private ObservableCollection<CalendarViewModel.IAvailability> _availabilites;

        public ObservableCollection<CalendarViewModel.IAvailability> Availabilities
        {
            get
            {
                lock (mutex)
                {
                    return _availabilites;
                }
            }
        }

        private void onAvailabilitesChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<CalendarModel.IAvailability> senderCollection = sender as ObservableCollection<CalendarModel.IAvailability>;
            var newAvailabilities = senderCollection.ToList();
            List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarModel.IAvailability, IAvailability>(Convert));

            lock (mutex)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in e.NewItems)
                    {
                        _availabilites.Add(Convert((CalendarModel.IAvailability)item));
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    _availabilites.Clear();
                }
            }

        }

        public static IAvailability Convert(CalendarModel.IAvailability a)
        {
            return new Availability(a);
        }

        public ViewModel()
        {
            calendarModel = new CalendarModel.CalendarModel();

            _availabilites = new ObservableCollection<IAvailability>();
            ActiveEmployeeId = 0;

            AddCommand = new Updater(o => AddButtonClick("Add"));

            BindingOperations.EnableCollectionSynchronization(_availabilites, mutex);
        }

        public ViewModel(CalendarModel.ICalendarModel model)
        {
            calendarModel = model;

            _availabilites = new ObservableCollection<IAvailability>();
            ActiveEmployeeId = 0;

            AddCommand = new Updater(o => AddButtonClick("Add"));

            BindingOperations.EnableCollectionSynchronization(_availabilites, mutex);
        }

        private void AddButtonClick(object sender)
        {
            calendarModel.AddActiveEmployeeAvailability(currentAvailability, currentAvailability);
            //calendarModel._employeeAvailabilityManager.ActiveEmployeeId = ActiveEmployeeId;
            //calendarModel._employeeAvailabilityManager.addAvailability(currentAvailability, currentAvailability);
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ICommand mUpdater;

        public ICommand AddCommand
        {
            get
            {
                return mUpdater;
            }
            set
            {
                mUpdater = value;
            }
        }

        public int ActiveEmployeeId
        {
            get { return calendarModel.getActiveEmployeeId(); }
            set {
                lock (mutex)
                {
                    Availabilities.Clear();
                    calendarModel.availabilities().CollectionChanged -= onAvailabilitesChange;
                    calendarModel.setActiveEmployeeId(value);


                    List<CalendarModel.IAvailability> newAvailabilities = calendarModel.availabilities().ToList();
                    List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarModel.IAvailability, IAvailability>(Convert));


                    foreach (var a in newLogicAvailabilities)
                    {
                        _availabilites.Add(a);
                    }

                    calendarModel.availabilities().CollectionChanged += onAvailabilitesChange;
                }
            }
        }

        public ObservableCollection<CalendarViewModel.IAvailability> ActiveEmployeeAvailabilities
        {
            get { return Availabilities; }
        }

        public DateTime MarkedAvailability
        {
            get
            {
                return this.currentAvailability;
            }

            set
            {
                if (value != this.currentAvailability)
                {
                    this.currentAvailability = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
