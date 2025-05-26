using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Victor.Training.Cleancode.Tests
{
    public class SplitLoopTests
    {
      
        [Test]
        public void ComputesCorrectAverageSalaryAndTotalConsultantSalary()
        {
            var employees = new List<Employee>
            {
                new Employee(1, 24, 2000, false),
                new Employee(2, 27, 2000, false),
                new Employee(3, 28, 1500, true),
                new Employee(4, 30, 2500, true)
            };

            var result = new SplitLoop().ComputeStats(employees);

            Assert.AreEqual("Total consultant salary: 4000; ids: 1, 2, 3, 4", result);
        }

        [Test]
        public void ReturnsZeroForEmptyEmployeeList()
        {
            var employees = new List<Employee>();

            var result = new SplitLoop().ComputeStats(employees);

            Assert.AreEqual("Total consultant salary: 0; ids: ", result);
        }

        [Test]
        public void ComputesCorrectlyWithAllConsultants()
        {
            var employees = new List<Employee>
            {
                new Employee(1, 24, 3000, true),
                new Employee(2, 27, 4000, true)
            };

            var result = new SplitLoop().ComputeStats(employees);

            Assert.AreEqual("Total consultant salary: 7000; ids: 1, 2", result);
        }

        [Test]
        public void ComputesCorrectlyWithNoConsultants()
        {
            var employees = new List<Employee>
            {
                new Employee(1, 24, 2000, false),
                new Employee(2, 27, 2500, false)
            };

            var result = new SplitLoop().ComputeStats(employees);

            Assert.AreEqual("Total consultant salary: 0; ids: 1, 2", result);
        }
    }
}