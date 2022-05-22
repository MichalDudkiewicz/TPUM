using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CalendarData.Test
{
    [TestClass]
    public class CalendarDataTest
    {
        public IEmployee employee;

        [TestInitialize]
        public void TestInit()
        {
            List<IEmployee> employees = new List<IEmployee>();

            employee = new Employee(0);
            employees.Add(employee);
        }

        [TestMethod]
        public void AddAvailability_Test()
        {
            employee.addAvailability(System.DateTime.Today, System.DateTime.Today.AddDays(2));
            employee.addAvailability(System.DateTime.Today.AddDays(5), System.DateTime.Today.AddDays(8));

            bool isFilled = employee.Availabilities().Count == 2;
            Assert.IsTrue(isFilled);
        }

        [TestMethod]
        public void RemoveAvailability_Test()
        {
            employee.addAvailability(System.DateTime.Today, System.DateTime.Today.AddDays(2));
            employee.addAvailability(System.DateTime.Today.AddDays(5), System.DateTime.Today.AddDays(8));

            bool dateCheck = employee.Availabilities()[0].startTime() == System.DateTime.Today;
            Assert.IsTrue(dateCheck);

            System.Guid availabilityId = employee.Availabilities()[0].id();

            employee.removeAvailability(availabilityId);

            bool isAvailabilityRemoved = employee.Availabilities().Count == 1;
            Assert.IsTrue(isAvailabilityRemoved);
        }

    }
}
