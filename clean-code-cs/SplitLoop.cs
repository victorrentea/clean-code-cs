
using System;
using System.Collections.Generic;
using System.Linq;

namespace Victor.Training.Cleancode
{
    public class SplitLoop
    {
        public string ComputeStats(List<Employee> employees)
        {
            double totalConsultantSalary = 0;
            foreach (var employee in employees)
            { 
                if (employee.Consultant)
                    totalConsultantSalary += employee.Salary ?? 0;
            }

            //double totalConsultantSalary = employees
            //    .Where(e => e.Consultant)
            //    .Select(e => e.Salary ?? 0)
            //    .Sum();


            //List<int?> employeeIds = new List<int?>();
            //foreach (var employee in employees)
            //{
            //    employeeIds.Add(employee.Id);
            //}
            List<int?> employeeIds = employees.Select(e => e.Id).ToList();

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

