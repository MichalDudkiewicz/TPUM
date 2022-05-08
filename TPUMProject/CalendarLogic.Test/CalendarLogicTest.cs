using CalendarData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CalendarLogic.Test
{
    [TestClass]
    public class CalendarLogicTest
    {
        EmployeeRepository mRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            List<IEmployee> employees = new List<IEmployee>();

            IEmployee employee = new Employee(0);
            employees.Add(employee);

            IEmployee employee2 = new Employee(1);
            employees.Add(employee2);

            mRepository = new EmployeeRepository(employees);
        }

        [TestMethod]
        public void SetActiveEmployee_GetExpectedId()
        {
            int expectedId = 1;
            IEmployeeAvailabilityManager employeeAvailabilityManager = new EmployeeAvailabilityManager(mRepository);
            employeeAvailabilityManager.setActiveEmployeeId(expectedId);
            Assert.AreEqual(employeeAvailabilityManager.getActiveEmployeeId(), expectedId);
        }

        [TestMethod]
        public void AddAvailability_ExpectedAvailabilityAdded()
        {
            IEmployeeAvailabilityManager employeeAvailabilityManager = new EmployeeAvailabilityManager(mRepository);
            DateTime date = new DateTime(2022, 5, 11);
            var availabilitites = employeeAvailabilityManager.getAvailabilities();
            Assert.AreEqual(availabilitites.Count, 0);
            employeeAvailabilityManager.addAvailability(date, date);
            Assert.AreEqual(availabilitites.Count, 1);
            Assert.AreEqual(availabilitites[0].startTime(), date);
            Assert.AreEqual(availabilitites[0].endTime(), date);
        }

        [TestMethod]
        public void RemoveAvailability_ExpectedAvailabilityRemove()
        {
            IEmployeeAvailabilityManager employeeAvailabilityManager = new EmployeeAvailabilityManager(mRepository);
            DateTime date = new DateTime(2022, 5, 11);
            employeeAvailabilityManager.addAvailability(date, date);
            var availabilitites = employeeAvailabilityManager.getAvailabilities();
            Assert.AreEqual(availabilitites.Count, 1);
            employeeAvailabilityManager.removeAvailability(availabilitites[0].id());
            Assert.AreEqual(0, availabilitites.Count);
        }

        [TestMethod]
        public void RemoveAvailability_UnexpectedAvailabilityNotRemoved()
        {
            IEmployeeAvailabilityManager employeeAvailabilityManager = new EmployeeAvailabilityManager(mRepository);
            DateTime date = new DateTime(2022, 5, 11);
            employeeAvailabilityManager.addAvailability(date, date);
            var availabilitites = employeeAvailabilityManager.getAvailabilities();
            Assert.AreEqual(availabilitites.Count, 1);
            employeeAvailabilityManager.removeAvailability(availabilitites[0].id());
            Assert.AreEqual(0, availabilitites.Count);
        }
    }
}
