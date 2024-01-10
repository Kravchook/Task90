using NUnit.Allure.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Task50.Tests
{
    [AllureNUnit]
    public class WaitForUserTest
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

            Driver.Navigate().GoToUrl("https://demo.seleniumeasy.com/dynamic-data-loading-demo.html");
        }

        [Test]
        public void WaitForUser()
        {
            var getNewUserButton = Driver.FindElement(By.XPath("//button[@id='save']"));
            for (int i = 0; i < 5; i++)
            {
                getNewUserButton.Click();

                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(7));

                var isUserImageDisplayed = wait.Until(condition =>
                {
                    try
                    {
                        var userImage = Driver.FindElement(By.XPath($"//div[@id='loading']/img"));
                        return userImage.Displayed;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                });

                Assert.That(isUserImageDisplayed, "Error: User image is not displayed!");
            }
        }

        [TearDown]
        public void Cleanup()
        {
            Driver.Dispose();
        }
    }
}
