using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalendarData;

namespace CalendarData.Test
{
    [TestClass]
    public class UnitTest1
    {
        private EmployeeRepository employeeRepository;

        [TestMethod]
        public void AddAvailability_Test()
        {
            employeeRepository = new EmployeeRepository();

            employeeRepository.GetById(0).addAvailability(System.DateTime.Today, System.DateTime.Today.AddDays(2));
            employeeRepository.GetById(0).addAvailability(System.DateTime.Today.AddDays(5), System.DateTime.Today.AddDays(8));

            employeeRepository.GetById(1).addAvailability(System.DateTime.Today, System.DateTime.Today.AddDays(2));

            bool isFirstFilled = employeeRepository.GetById(0).Availabilities.Count == 2;
            Assert.IsTrue(isFirstFilled);

            bool isSecondFilled = employeeRepository.GetById(1).Availabilities.Count == 1;
            Assert.IsTrue(isSecondFilled);
        }

        [TestMethod]
        public void RemoveAvailability_Test()
        {
            employeeRepository = new EmployeeRepository();

            employeeRepository.GetById(0).addAvailability(System.DateTime.Today, System.DateTime.Today.AddDays(2));
            employeeRepository.GetById(0).addAvailability(System.DateTime.Today.AddDays(5), System.DateTime.Today.AddDays(8));

            bool dateCheck = employeeRepository.GetById(0).Availabilities[0].StartTime == System.DateTime.Today;
            Assert.IsTrue(dateCheck);

            System.Guid availabilityId = employeeRepository.GetById(0).Availabilities[0].Id;

            employeeRepository.GetById(0).removeAvailability(availabilityId);

            bool isAvailabilityRemoved = employeeRepository.GetById(0).Availabilities.Count == 1;
            Assert.IsTrue(isAvailabilityRemoved);
        }

        [TestMethod]
        public void ChangeAvailabilityValue_Test()
        {
            employeeRepository = new EmployeeRepository();

            employeeRepository.GetById(0).addAvailability(System.DateTime.Today, System.DateTime.Today.AddDays(2));

            bool dateCheck = employeeRepository.GetById(0).Availabilities[0].StartTime == System.DateTime.Today;
            Assert.IsTrue(dateCheck);

            employeeRepository.GetById(0).Availabilities[0].setStartTime(System.DateTime.Today.AddDays(2));
            employeeRepository.GetById(0).Availabilities[0].setEndTime(System.DateTime.Today.AddDays(5));

            bool startTimeCheck = employeeRepository.GetById(0).Availabilities[0].StartTime == System.DateTime.Today;
            Assert.IsFalse(startTimeCheck);

            bool endTimeCheck = employeeRepository.GetById(0).Availabilities[0].EndTime == System.DateTime.Today.AddDays(2);
            Assert.IsFalse(endTimeCheck);

        }

    }
}
