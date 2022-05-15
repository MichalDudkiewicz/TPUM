using CalendarData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalendarLogic.Test
{
    [TestClass]
    public class CalendarLogicIntergrationTest
    {
        private IRepository<IEmployee> mRepository;
        private Task val = null;

        private async Task LongRunningMethod()
        {
            await mRepository.connect();
        }

        [TestInitialize]
        public void TestInitialize()
        {
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
        }
    }
}
