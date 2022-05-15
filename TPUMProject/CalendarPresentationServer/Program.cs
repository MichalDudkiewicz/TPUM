﻿using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using CalendarViewModelServer;

namespace CalendarViewServer
{
    internal class Program
    {
        static WebSocketConnection _wserver = null;
        static async Task Main(string[] args)
        {
            Uri uri = new Uri("ws://localhost:6966");
            ICalendarViewModel calendarViewModel = new ViewModel();
            Task server = Task.Run(async () => await WebSocketServer.Server(uri.Port,
                _ws =>
                {
                    _wserver = _ws; _wserver.onMessage = (data) =>
                    {
                        Console.WriteLine("------------");
                        Console.WriteLine("[RECEIVED]:");
                        Console.WriteLine(data.ToString());
                        Console.WriteLine("------------");
                        calendarViewModel.receiveData(data);
                    };
                }));

            calendarViewModel.SendData += updateClientAvailabilities;

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