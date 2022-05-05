using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using CalendarData;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CalendarViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private CalendarModel.CalendarModel calendarModel;
        private DateTime currentAvailability;
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

            //calendarModel._employeeAvailabilityManager.addAvailability(DateTime.Today, new DateTime(2022, 4, 27));
            calendarModel._employeeAvailabilityManager.ActiveEmployeeId = ActiveEmployeeId;
            calendarModel._employeeAvailabilityManager.addAvailability(currentAvailability, currentAvailability);
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
            get { return calendarModel.ActiveEmployeeId; }
            set { calendarModel.ActiveEmployeeId = value; }
        }

        public ObservableCollection<Availability> ActiveEmployeeAvailabilities
        {
            get { return calendarModel.ActiveEmployeeAvailabilities; }
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
