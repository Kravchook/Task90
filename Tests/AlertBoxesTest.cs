using NUnit.Allure.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Task50.Tests
{
    [AllureNUnit]
    public class AlertBoxesTest
    {
        public WebDriver Driver { get; set; }

        private const string ExpectedAlertText = "I am an alert box!";
        private const string ExpectedConfirmText = "Press a button!";
        private const string ExpectedPromptText = "Please enter your name";

        [SetUp]
        public void Setup()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(7);
            //Add implicit waiter for WebDriver
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            Driver.Navigate().GoToUrl("https://demo.seleniumeasy.com/javascript-alert-box-demo.html");
        }

        [Test]
        public void AlertBoxTest()
        {
            var alertBoxButton = Driver.FindElement(By.XPath("//button[contains(@onclick, 'Alert')]"));
            alertBoxButton.Click();

            var alertBox = Driver.SwitchTo().Alert();
            Assert.That(alertBox.Text, Is.EqualTo(ExpectedAlertText), "Alert box text is not correct!");
            Thread.Sleep(3000);
            alertBox.Accept();
        }

        [Test]
        public void ConfirmBoxTest()
        {
            var confirmBoxButton = Driver.FindElement(By.XPath("//button[contains(@onclick, 'Confirm')]"));

            //Cancel click scenario
            confirmBoxButton.Click();
            var confirmBox = Driver.SwitchTo().Alert();
            Assert.That(confirmBox.Text, Is.EqualTo(ExpectedConfirmText), "Confirm box text is not correct!");
            Thread.Sleep(3000);
            confirmBox.Dismiss();

            //OK click scenario
            confirmBoxButton.Click();
            confirmBox = Driver.SwitchTo().Alert();
            Assert.That(confirmBox.Text, Is.EqualTo(ExpectedConfirmText), "Confirm box text is not correct!");
            Thread.Sleep(3000);
            confirmBox.Accept();
        }

        [Test]
        public void PromptBoxTest()
        {
            var promptBoxButton = Driver.FindElement(By.XPath("//button[contains(@onclick, 'Prompt')]"));

            //Cancel click scenario
            promptBoxButton.Click();
            var promptBox = Driver.SwitchTo().Alert();
            Assert.That(promptBox.Text, Is.EqualTo(ExpectedPromptText), "Prompt box text is not correct!");
            Thread.Sleep(1000);
            Driver.SwitchTo().Alert().SendKeys("Cool guy");
            Thread.Sleep(2000);
            Driver.SwitchTo().Alert().Dismiss();

            //OK click scenario
            promptBoxButton.Click();
            promptBox = Driver.SwitchTo().Alert();
            Assert.That(promptBox.Text, Is.EqualTo(ExpectedPromptText), "Prompt box text is not correct!");
            Thread.Sleep(1000);
            promptBox.SendKeys("Other guy");
            Thread.Sleep(2000);
            promptBox.Accept();
        }

        [TearDown]
        public void Cleanup()
        {
            Driver.Dispose();
        }
    }
}
