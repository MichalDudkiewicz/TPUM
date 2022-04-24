using System;
using System.Windows.Input;
using System.ComponentModel;
using CalendarModel;
using CalendarData;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace CalendarViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private CalendarModel.CalendarModel calendarModel;
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel(CalendarModel.CalendarModel newCalendarModel)
        {
            calendarModel = newCalendarModel;
            AddCommand = new Updater(o => AddButtonClick("Add"));
        }

        private void AddButtonClick(object sender)
        {
            Availability av = new Availability(DateTime.Today, new DateTime(2022, 4, 27));
            //ActiveEmployeeAvailabilities.Add(av);

            calendarModel._employeeAvailabilityManager.addAvailability(DateTime.Today, new DateTime(2022, 4, 27));

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
            get { return calendarModel.ActiveEmployeeId; }
            set { calendarModel.ActiveEmployeeId = value; }
        }

        public ObservableCollection<Availability> ActiveEmployeeAvailabilities
        {
            get { return calendarModel.ActiveEmployeeAvailabilities; }
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
