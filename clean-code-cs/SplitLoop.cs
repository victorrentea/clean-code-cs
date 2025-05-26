
    using System;
    using System.Collections.Generic;
    using System.Linq;

    namespace Victor.Training.Cleancode
    {
        public class SplitLoop
        {
            // TODO Split loops and refactor to LINQ. Run Tests✅
            public string ComputeStats(List<Employee> employees)
            {
                List<int?> employeeIds = new List<int?>();
                double totalConsultantSalary = 0;
                foreach (var employee in employees)
                {
                    if (employee.Consultant)
                    {
                        totalConsultantSalary += employee.Salary ?? 0;
                    }
                    employeeIds.Add(employee.Id);
                }
                Console.WriteLine("Employee IDs: " + string.Join(", ", employeeIds));
                return $"Total consultant salary: {totalConsultantSalary}; ids: {string.Join(", ", employeeIds)}";
            }
        }

        public class Employee
        {
            public int? Id { get; set; }
            public int Age { get; set; }
            public int? Salary { get; set; }
            public bool Consultant { get; set; }

            public Employee(int? id, int age, int? salary, bool consultant)
            {
                Id = id;
                Age = age;
                Salary = salary;
                Consultant = consultant;
            }
        }
    }

