using System;
using System.Windows.Input;
using System.ComponentModel;
using CalendarModel;
using CalendarData;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace CalendarViewModel
{
    public class ViewModel
    {
        private CalendarModel.CalendarModel calendarModel;
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel(CalendarModel.CalendarModel newCalendarModel)
        {
            calendarModel = newCalendarModel;
        }

        public ViewModel()
        {
            //calendarModel = new CalendarModel.CalendarModel();
        }

        private ICommand mUpdater;

        public ICommand AddCommand
        {
            get
            {
                if (mUpdater == null)
                    mUpdater = new Updater();
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
            set { calendarModel.ActiveEmployeeAvailabilities = value; }
        }
    }

    class Updater : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            
        }
    }

}
