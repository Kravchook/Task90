using NUnit.Allure.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Task50.Tests
{
    [AllureNUnit]
    public class DownloadProgressTest
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

            Driver.Navigate().GoToUrl("https://demo.seleniumeasy.com/bootstrap-download-progress-demo.html");
        }

        [Test]
        public void DownloadProgress()
        {
            var downloadButton = Driver.FindElement(By.XPath("//button[@id='cricle-btn']"));
            downloadButton.Click();

            var progressBar = Driver.FindElement(By.XPath("//div[@class='percenttext']"));
            while (progressBar.Text != "100%")
            {
                var values = progressBar.Text.Split('%');
                if (int.Parse(values[0]) >= 50)
                {
                    Driver.Navigate().Refresh();
                    break;
                }
            }
        }

        [TearDown]
        public void Cleanup()
        {
            Driver.Dispose();
        }
    }
}
