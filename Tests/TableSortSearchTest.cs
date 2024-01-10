using NUnit.Allure.Core;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Task50.Tests
{
    [AllureNUnit]
    public class TableSortSearchTest
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
        public void TableSortSearch()
        {
            // Select “10” option in dropdown “Show () entries”
            var showElement = Driver.FindElement(By.XPath("//select[@name='example_length']"));

            SelectElement showEntries = new SelectElement(showElement);
            showEntries.SelectByValue("10");

            var sortedData = GetSortedDataByAgeAndSalary(50, 400000);

            Console.WriteLine("Custom Data: ");
            foreach (var item in sortedData)
            {
                Console.WriteLine("Row: " + item);
            }
        }

        private List<string> GetSortedDataByAgeAndSalary(int age, int salary)
        {
            var sortedDataList = new List<string>();

            // Iterations through all pages
            var pageNumButtons = Driver.FindElements(By.XPath("//span/a[contains(@class, 'paginate_button')]")).ToList();

            for (int i = 1; i < pageNumButtons.Count; i++)
            {
                var rowElements = Driver.FindElements(By.XPath("//tbody/tr")).ToList();
                foreach (var rowElement in rowElements)
                {
                    var rowCellElements = rowElement.FindElements(By.XPath("./td")).ToList();
                    var salaryAsString = rowCellElements.ElementAt(5).Text.Remove(0, 1)
                        .Replace(",", "").Replace("/y", "");

                    if (int.Parse(rowCellElements.ElementAt(3).Text) > age && int.Parse(salaryAsString) <= salary)
                    {
                        var employeeName = rowCellElements.ElementAt(0).Text;
                        var employeePosition = rowCellElements.ElementAt(1).Text;
                        var employeeOffice = rowCellElements.ElementAt(2).Text;
                        var employeeAge = rowCellElements.ElementAt(3).Text;
                        var employeeStartDate = rowCellElements.ElementAt(4).Text;
                        var employeeSalary = rowCellElements.ElementAt(5).Text;
                        sortedDataList.Add(string.Format($"{employeeName} {employeePosition} {employeeOffice} {employeeAge} " +
                                                         $"{employeeStartDate} {employeeSalary}"));
                    }
                }

                var nextButton = Driver.FindElement(By.XPath("//a[contains(@class, 'next')]"));
                nextButton.Click();
            }

            return sortedDataList;
        }

        [TearDown]
        public void Cleanup()
        {
            Driver.Dispose();
        }
    }
}
