namespace Task50
{
    public class Employee
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Office { get; set; }
        public string Age { get; set; }
        public string StartDate { get; set; }
        public string Salary { get; set; }

        public Employee(string name, string position, string office, string age, string startDate, string salary)
        {
            Name = name;
            Position = position;
            Office = office;
            Age = age;
            StartDate = startDate;
            Salary = salary;
        }

        public Employee()
        {

        }

        public List<string> GetSortedDataByAgeAndSalary(List<Employee> employees, int age, int salary)
        {
            var sortedDataList = new List<string>();

            foreach (var employee in employees)
            {
                var salaryAsString = employee.Salary.Remove(0, 1)
                    .Replace(",", "").Replace("/y", "");

                if (int.Parse(employee.Age) > age && int.Parse(salaryAsString) <= salary)
                {
                    sortedDataList.Add(string.Format($"{employee.Name} {employee.Position} {employee.Office} {employee.Age} " +
                                                     $"{employee.StartDate} {employee.Salary}"));
                }
            }

            return sortedDataList;
        }
    }
}
