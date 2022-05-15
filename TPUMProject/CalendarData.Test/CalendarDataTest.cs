using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CalendarData.Test
{
    [TestClass]
    public class CalendarDataTest
    {
        private IRepository<IEmployee> employeeRepository;

        [TestInitialize]
        public void TestInit()
        {
            List<IEmployee> employees = new List<IEmployee>();

            IEmployee employee = new Employee(0);
            employees.Add(employee);

            IEmployee employee2 = new Employee(1);
            employees.Add(employee2);

            employeeRepository = new EmployeeRepository(employees);

        }

        [TestMethod]
        public void AddAvailability_Test()
        {
            employeeRepository.GetById(0).addAvailability(System.DateTime.Today, System.DateTime.Today.AddDays(2));
            employeeRepository.GetById(0).addAvailability(System.DateTime.Today.AddDays(5), System.DateTime.Today.AddDays(8));

            employeeRepository.GetById(1).addAvailability(System.DateTime.Today, System.DateTime.Today.AddDays(2));

            bool isFirstFilled = employeeRepository.GetById(0).Availabilities().Count == 2;
            Assert.IsTrue(isFirstFilled);

            bool isSecondFilled = employeeRepository.GetById(1).Availabilities().Count == 1;
            Assert.IsTrue(isSecondFilled);
        }

        [TestMethod]
        public void RemoveAvailability_Test()
        {
            employeeRepository.GetById(0).addAvailability(System.DateTime.Today, System.DateTime.Today.AddDays(2));
            employeeRepository.GetById(0).addAvailability(System.DateTime.Today.AddDays(5), System.DateTime.Today.AddDays(8));

            bool dateCheck = employeeRepository.GetById(0).Availabilities()[0].startTime() == System.DateTime.Today;
            Assert.IsTrue(dateCheck);

            System.Guid availabilityId = employeeRepository.GetById(0).Availabilities()[0].id();

            employeeRepository.GetById(0).removeAvailability(availabilityId);

            bool isAvailabilityRemoved = employeeRepository.GetById(0).Availabilities().Count == 1;
            Assert.IsTrue(isAvailabilityRemoved);
        }

    }
}
