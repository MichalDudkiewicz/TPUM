using System;
using System.Threading.Tasks;
using CalendarViewModelServer;

#if (DEBUG)
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("CalendarLogic.Test")]
#endif

namespace CalendarViewServer
{

    internal class Program
    {
        static WebSocketConnection _wserver = null;
        static async Task Main(string[] args)
        {
            Uri uri = new Uri("ws://localhost:6966");
            ICalendarViewModel dataContext = new ViewModel();
            Task server = Task.Run(async () => await WebSocketServer.Server(uri.Port,
                _ws =>
                {
                    _wserver = _ws; _wserver.onMessage = (data) =>
                    {
                        Console.WriteLine("------------");
                        Console.WriteLine("[RECEIVED]:");
                        Console.WriteLine(data.ToString());
                        Console.WriteLine("------------");
                        dataContext.receiveData(data);
                    };
                }));

            dataContext.SendData += updateClientAvailabilities;

            Console.ReadKey();
        }
        
        private async static void updateClientAvailabilities(object sender, string e)
        {
            if (_wserver != null)
            {
                Console.WriteLine("------------");
                Console.WriteLine("[SENT]:");
                Console.WriteLine(e);
                Console.WriteLine("------------");
                await _wserver.SendAsync(e);
            }
        }        
    }
}
