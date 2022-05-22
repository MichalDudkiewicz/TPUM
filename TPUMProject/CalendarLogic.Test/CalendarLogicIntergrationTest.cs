using CalendarData;
using CalendarViewModelServer;
using CalendarViewServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace CalendarLogic.Test
{
    [TestClass]
    public class CalendarLogicIntergrationTest
    {
        IEmployeeDataManager employeeDataManager;

        static CalendarViewServer.WebSocketConnection _wserver = null;

        private Task val = null;

        private async Task LongRunningMethod()
        {
            await employeeDataManager.connect();
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

        [TestInitialize]
        public void TestInitialize()
        {
            employeeDataManager = new EmployeeDataManager(0);

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

            val = LongRunningMethod();
        }

        [TestMethod]
        public async Task AddAvailability_ExpectedAvailabilityAdded()
        {
            DateTime date = new DateTime(2022, 5, 11);
            var availabilitites = employeeDataManager.Availabilities();
            Assert.AreEqual(availabilitites.Count, 0);

            CalendarData.IAvailability newAvailability = new CalendarData.Availability(date, date);
            IAvailability availability = new Availability(newAvailability);

            employeeDataManager.AddAvailability(availability.id(),availability.startTime(),availability.endTime());

            await Task.Delay(1000);

            bool contains = false;
            foreach (var avail in availabilitites)
            {
                if (avail.startTime() == date && avail.endTime() == date)
                {
                    contains = true;
                    break;
                }
            }
            Assert.IsTrue(contains);

            await _wserver?.DisconnectAsync();
        }

        [TestMethod]
        public async Task RemoveAvailability_ExpectedAvailabilityRemove()
        {
            //DateTime date = new DateTime(2022, 5, 11);

            //CalendarData.IAvailability newAvailability = new CalendarData.Availability(date, date);
            //IAvailability availability = new Availability(newAvailability);

            //employeeDataManager.AddAvailability(availability.id(), availability.startTime(), availability.endTime());

            await Task.Delay(1000);

            //var availabilitites = employeeDataManager.Availabilities();
            //Assert.AreEqual(availabilitites.Count, 1);

            //employeeDataManager.removeAvailability(availability.id());

            await Task.Delay(1000);

            //bool contains = false;
            //foreach (var avail in availabilitites)
            //{
            //    if (avail.startTime() == date && avail.endTime() == date && avail.id() == availability.id())
            //    {
            //        contains = true;
            //        break;
            //    }
            //}
            //Assert.IsFalse(contains);

            //await _wserver?.DisconnectAsync();
        }
    }
}
