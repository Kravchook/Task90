using NUnit.Allure.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Task50.Tests
{
    [AllureNUnit]
    public class MultiselectControlTest
    {
        public WebDriver Driver { get; set; }

        [SetUp]
        public void Setup()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(7);
            //Add implicit waiter for WebDriver
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            Driver.Navigate().GoToUrl("https://demo.seleniumeasy.com/basic-select-dropdown-demo.html");
        }

        [Test]
        public void MultiselectTest()
        {
            var multiselect = Driver.FindElement(By.XPath("//select[@id='multi-select']"));

            SelectElement select = new SelectElement(multiselect);

            //Creating list of expected selections (3 random options)
            var expectedSelectedOptionsList = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                var randomOption = GetRandomOption(multiselect);
                select.SelectByValue(randomOption.Text);
                Thread.Sleep(1000);

                expectedSelectedOptionsList.Add(randomOption.Text);
            }
            expectedSelectedOptionsList.Sort();

            //Creating list of actual selections
            var actualSelectedOptionsList = new List<string>();
            foreach (var webElement in select.AllSelectedOptions.ToList())
            {
                actualSelectedOptionsList.Add(webElement.Text);
            }
            actualSelectedOptionsList.Sort();

            Assert.That(actualSelectedOptionsList, Is.EqualTo(expectedSelectedOptionsList), "Lists are not equal!");
        }

        [TearDown]
        public void Cleanup()
        {
            Driver.Dispose();
        }

        private IWebElement GetRandomOption(IWebElement multiselect)
        {
            Random random = new Random();
            var allOptions = multiselect.FindElements(By.XPath("//select[@id='multi-select']/option"));
            var notSelectedOptions = new List<IWebElement>();

            foreach (var option in allOptions)
            {
                if (!option.Selected)
                {
                    notSelectedOptions.Add(option);
                }
            }
            var randomOption = notSelectedOptions[random.Next(notSelectedOptions.Count)];

            return randomOption;
        }
    }
}
