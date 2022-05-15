using CalendarData;
using CalendarViewModelServer;
using CalendarViewServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalendarLogic.Test
{
    [TestClass]
    public class CalendarLogicIntergrationTest
    {
        static CalendarViewServer.WebSocketConnection _wserver = null;

        private IRepository<IEmployee> mRepository;
        private Task val = null;

        private async Task LongRunningMethod()
        {
            await mRepository.connect();
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

            List<IEmployee> employees = new List<IEmployee>();

            IEmployee employee = new Employee(0);
            employees.Add(employee);

            IEmployee employee2 = new Employee(1);
            employees.Add(employee2);

            mRepository = new EmployeeRepository(employees);

            val = LongRunningMethod();
        }

        [TestMethod]
        public async Task AddAvailability_ExpectedAvailabilityAdded()
        {
            IEmployeeAvailabilityManager employeeAvailabilityManager = new EmployeeAvailabilityManager(mRepository);
            DateTime date = new DateTime(2022, 5, 11);
            var availabilitites = employeeAvailabilityManager.getAvailabilities();
            Assert.AreEqual(availabilitites.Count, 0);
            employeeAvailabilityManager.addAvailability(date, date);

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
            IEmployeeAvailabilityManager employeeAvailabilityManager = new EmployeeAvailabilityManager(mRepository);
            DateTime date = new DateTime(2022, 5, 11);
            employeeAvailabilityManager.addAvailability(date, date);

            await Task.Delay(1000);

            var availabilitites = employeeAvailabilityManager.getAvailabilities();
            Assert.AreEqual(availabilitites.Count, 1);

            employeeAvailabilityManager.removeAvailability(availabilitites[0].id());

            await Task.Delay(1000);

            availabilitites = employeeAvailabilityManager.getAvailabilities();
            Assert.AreEqual(0, availabilitites.Count);

            await _wserver?.DisconnectAsync();
        }
    }
}
