using System.Windows;
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
            ICalendarViewModel calendarViewModel = new ViewModel();

            main.DataContext = calendarViewModel;
            main.Show();
        }
    }
}
