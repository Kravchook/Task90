using NUnit.Allure.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Task50.Tests
{
    [AllureNUnit]
    public class ParameterizedTest2
    {
        const string Email1 = "testerautomation601@gmail.com";
        const string Password1 = "myINVULNERABLEpass";
        const string Email2 = "testerautomation602@gmail.com";
        const string Password2 = "myINVULNERABLEpass2";

        [Test]
        [TestCase(Email1, Password1)]
        [TestCase(Email2, Password2)]
        [Parallelizable(ParallelScope.All)]
        public void GoogleLoginTest2(string login, string password)
        {
            var welcomeText = "Welcome, Automation Tester";

            var driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
            //Add implicit waiter for WebDriver
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            driver.Navigate().GoToUrl("https://accounts.google.com/");

            var loginInput = driver.FindElement(By.XPath("//input[@id='identifierId']"));
            loginInput.SendKeys(login);

            var nextBtn = driver.FindElement(By.XPath("//span[contains(text(), 'Next')]"));
            nextBtn.Click();

            // Explicit wait, because it waits for the condition (sleep 5 sec), doesn't matter if the page or element has been already loaded
            Thread.Sleep(5000);

            var passwordInput = driver.FindElement(By.XPath("//input[@name='Passwd']"));
            passwordInput.SendKeys(password);

            nextBtn = driver.FindElement(By.XPath("//span[contains(text(), 'Next')]"));
            nextBtn.Click();

            //Add explicit waiter for login test, which will wait until name appears (after login).
            //Add polling frequency, which is differ from default value
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(7))
            {
                PollingInterval = TimeSpan.FromMilliseconds(1000)
            };

            var isWelcomeLabelDisplayed = wait.Until(condition =>
            {
                try
                {
                    var welcomeLabel = driver.FindElement(By.XPath($"//*[contains(text(), '{welcomeText}')]"));
                    return welcomeLabel.Displayed;
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

            Assert.That(isWelcomeLabelDisplayed, "Error: Welcome text is not displayed!");

            // Terminates the remote webdriver session
            driver.Quit();
        }
    }
}