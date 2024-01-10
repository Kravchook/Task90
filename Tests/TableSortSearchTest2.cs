using NUnit.Allure.Core;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Task50.Tests
{
    [AllureNUnit]
    public class TableSortSearchTest2
    {
        public WebDriver Driver { get; set; }

        [SetUp]
        public void Setup()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            //Add implicit waiter for WebDriver
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            Driver.Navigate().GoToUrl("https://demo.seleniumeasy.com/table-sort-search-demo.html");
        }

        [Test]
        public void TableSortSearch2()
        {
            // Select “10” option in dropdown “Show () entries”
            var showElement = Driver.FindElement(By.XPath("//select[@name='example_length']"));

            SelectElement showEntries = new SelectElement(showElement);
            showEntries.SelectByValue("10");

            var employeesList = GetEmployeesList();
            Employee employee = new Employee();

            var sortedData = employee.GetSortedDataByAgeAndSalary(employeesList, 50, 400000);

            Console.WriteLine("Custom Data: ");
            foreach (var item in sortedData)
            {
                Console.WriteLine("Row: " + item);
            }
        }

        private List<Employee> GetEmployeesList()
        {
            var employeesList = new List<Employee>();

            // Iterations through all pages
            var pageNumButtons = Driver.FindElements(By.XPath("//span/a[contains(@class, 'paginate_button')]")).ToList();

            for (int i = 1; i < pageNumButtons.Count; i++)
            {
                var rowElements = Driver.FindElements(By.XPath("//tbody/tr")).ToList();
                foreach (var rowElement in rowElements)
                {
                    var rowCellElements = rowElement.FindElements(By.XPath("./td")).ToList();
                    
                        var employeeName = rowCellElements.ElementAt(0).Text;
                        var employeePosition = rowCellElements.ElementAt(1).Text;
                        var employeeOffice = rowCellElements.ElementAt(2).Text;
                        var employeeAge = rowCellElements.ElementAt(3).Text;
                        var employeeStartDate = rowCellElements.ElementAt(4).Text;
                        var employeeSalary = rowCellElements.ElementAt(5).Text;
                        employeesList.Add(new Employee(employeeName, employeePosition, employeeOffice, employeeAge, employeeStartDate, employeeSalary));
                }

                var nextButton = Driver.FindElement(By.XPath("//a[contains(@class, 'next')]"));
                nextButton.Click();
            }

            return employeesList;
        }

        [TearDown]
        public void Cleanup()
        {
            Driver.Dispose();
        }
    }
}
