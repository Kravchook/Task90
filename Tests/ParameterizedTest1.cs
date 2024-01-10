using NUnit.Allure.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Concurrent;

namespace Task50.Tests
{
    [AllureNUnit]
    public class ParameterizedTest1
    {
        private static readonly ConcurrentDictionary<int, IWebDriver> DriverCollection =
            new ConcurrentDictionary<int, IWebDriver>();

        const string Email1 = "testerautomation601@gmail.com";
        const string Password1 = "myINVULNERABLEpass";
        const string Email2 = "testerautomation602@gmail.com";
        const string Password2 = "myINVULNERABLEpass2";

        [SetUp]
        public void Setup()
        {
            // Initialize driver
            int threadId = Thread.CurrentThread.ManagedThreadId;
            if (!DriverCollection.TryAdd(threadId, new ChromeDriver()))
            {
                TerminateSession(Thread.CurrentThread);
                DriverCollection.TryAdd(threadId, new ChromeDriver());
            }

            GetDriver().Manage().Window.Maximize();
            GetDriver().Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
            //Add implicit waiter for WebDriver
            GetDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            GetDriver().Navigate().GoToUrl("https://accounts.google.com/");
        }

        [Test]
        [TestCase(Email1, Password1)]
        [TestCase(Email2, Password2)]
        [Parallelizable(ParallelScope.All)]
        public void GoogleLoginTest(string login, string password)
        {
            var welcomeText = "Welcome, Automation Tester";

            var loginInput = GetDriver().FindElement(By.XPath("//input[@id='identifierId']"));
            loginInput.SendKeys(login);

            var nextBtn = GetDriver().FindElement(By.XPath("//span[contains(text(), 'Next')]"));
            nextBtn.Click();

            // Explicit wait, because it waits for the condition (sleep 5 sec), doesn't matter if the page or element has been already loaded
            Thread.Sleep(5000);

            var passwordInput = GetDriver().FindElement(By.XPath("//input[@name='Passwd']"));
            passwordInput.SendKeys(password);

            nextBtn = GetDriver().FindElement(By.XPath("//span[contains(text(), 'Next')]"));
            nextBtn.Click();

            //Add explicit waiter for login test, which will wait until name appears (after login).
            //Add polling frequency, which is differ from default value
            var wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(7))
            {
                PollingInterval = TimeSpan.FromMilliseconds(1000)
            };

            var isWelcomeLabelDisplayed = wait.Until(condition =>
            {
                try
                {
                    var welcomeLabel = GetDriver().FindElement(By.XPath($"//*[contains(text(), '{welcomeText}')]"));
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
        }

        [TearDown]
        public void Cleanup()
        {
            bool passed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed;
            try
            {
                // Logs the result
                Console.WriteLine("test-status=" + (passed ? "passed" : "failed"));
            }
            finally
            {
                // Terminates the remote webdriver session
                //GetDriver().Quit();
                TerminateSession(Thread.CurrentThread);
            }
        }

        public static IWebDriver GetDriver()
        {
            DriverCollection.TryGetValue(Thread.CurrentThread.ManagedThreadId, out var webDriver);

            return webDriver;
        }

        public static void TerminateSession(Thread thread)
        {
            var webDriver = GetDriver();
            if (webDriver != null)
            {
                webDriver.Quit();
                webDriver = null;
            }

            DriverCollection.TryRemove(thread.ManagedThreadId, out webDriver);
        }
    }
}