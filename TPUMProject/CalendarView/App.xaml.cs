using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CalendarData;
using CalendarLogic;
using CalendarModel;
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
            List<Employee> employees = new List<Employee>();
            Employee first = new Employee();
            first.Id = 0;
            employees.Add(first);
            first.addAvailability(DateTime.Today, new DateTime(2022, 4, 27));
            EmployeeRepository repository = new EmployeeRepository(employees);
            IEmployeeAvailabilityManager employeeAvailabilityManager = new IEmployeeAvailabilityManager(repository);
            CalendarModel.CalendarModel model = new CalendarModel.CalendarModel(employeeAvailabilityManager);
            ViewModel calendarViewModel = new ViewModel(model);
            calendarViewModel.ActiveEmployeeId = 0;
            main.DataContext = calendarViewModel;
            main.Show();
        }
    }
}
