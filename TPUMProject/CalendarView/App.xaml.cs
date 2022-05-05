using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CalendarData;
using CalendarLogic;
using CalendarViewModel;

namespace CalendarView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CalendarView.MainWindow main = new CalendarView.MainWindow();

            EmployeeRepository repository = new EmployeeRepository();
            IEmployeeAvailabilityManager employeeAvailabilityManager = new IEmployeeAvailabilityManager(repository);
            CalendarModel.CalendarModel model = new CalendarModel.CalendarModel(employeeAvailabilityManager);
            ViewModel calendarViewModel = new ViewModel(model);
            calendarViewModel.ActiveEmployeeId = 0;
            main.DataContext = calendarViewModel;
            main.Show();
        }
    }
}
