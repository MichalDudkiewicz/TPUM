using CalendarData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CalendarLogic.Test
{
    [TestClass]
    public class CalendarLogicTest
    {
        private IRepository<IEmployee> mRepository;

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
    }
}
