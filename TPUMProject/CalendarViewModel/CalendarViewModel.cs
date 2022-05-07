﻿using System;
using System.Windows.Input;
using System.ComponentModel;
using CalendarData;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;

namespace CalendarViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private CalendarModel.CalendarModel calendarModel;
        private DateTime currentAvailability;
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<CalendarViewModel.Availability> _availabilites;

        public ObservableCollection<CalendarViewModel.Availability> Availabilities
        {
            get
            {
                return _availabilites;
            }
        }

        private void onAvailabilitesChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<CalendarModel.Availability> senderCollection = sender as ObservableCollection<CalendarModel.Availability>;
            var newAvailabilities = senderCollection.ToList();
            List<Availability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarModel.Availability, Availability>(Convert));
            
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    _availabilites.Add(Convert((CalendarModel.Availability)item));
                    MarkedAvailability = Convert((CalendarModel.Availability)item).startTime;
                }
            }
        }

        public static Availability Convert(CalendarModel.Availability a)
        {
            return new Availability(a);
        }

        public ViewModel()
        {
            calendarModel = new CalendarModel.CalendarModel();

            ActiveEmployeeId = 0;

            AddCommand = new Updater(o => AddButtonClick("Add"));

            _availabilites = new ObservableCollection<Availability>();
            calendarModel.ActiveEmployeeAvailabilities.CollectionChanged += onAvailabilitesChange;
        }

        private void AddButtonClick(object sender)
        {
            calendarModel.AddActiveEmployeeAvailability(currentAvailability, currentAvailability);
            calendarModel.ActiveEmployeeId = ActiveEmployeeId;
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
            get { return calendarModel.ActiveEmployeeId; }
            set { calendarModel.ActiveEmployeeId = value; }
        }

        public ObservableCollection<CalendarViewModel.Availability> ActiveEmployeeAvailabilities
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
