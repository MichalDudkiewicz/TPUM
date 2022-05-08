using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;

namespace CalendarViewModel
{
    public class ViewModel : ICalendarViewModel, INotifyPropertyChanged
    {
        private CalendarModel.ICalendarModel calendarModel;
        private DateTime currentAvailability;
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<CalendarViewModel.IAvailability> _availabilites;

        public ObservableCollection<CalendarViewModel.IAvailability> Availabilities
        {
            get
            {
                return _availabilites;
            }
        }

        private void onAvailabilitesChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<CalendarModel.IAvailability> senderCollection = sender as ObservableCollection<CalendarModel.IAvailability>;
            var newAvailabilities = senderCollection.ToList();
            List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarModel.IAvailability, IAvailability>(Convert));
            
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


        }

        public ViewModel(CalendarModel.ICalendarModel model)
        {
            calendarModel = model;

            _availabilites = new ObservableCollection<IAvailability>();
            ActiveEmployeeId = 0;

            AddCommand = new Updater(o => AddButtonClick("Add"));


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

        static void Main(string[] args)
        {

        }

        public int ActiveEmployeeId
        {
            get { return calendarModel.getActiveEmployeeId(); }
            set {
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

    class Updater : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public Updater(Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null) throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            _execute(parameter ?? "<N/A>");
        }
    }

}
