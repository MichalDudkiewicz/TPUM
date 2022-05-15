using System;
using System.Collections.ObjectModel;

namespace CalendarViewModelServer
{
    public interface ICalendarViewModel
    {
        void receiveData(string message);

        event EventHandler<string> SendData;
    }
}
