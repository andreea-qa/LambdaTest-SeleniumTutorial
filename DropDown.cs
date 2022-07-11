using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace SeleniumTutorial
{
    public class DropDown
    {
        private static IWebDriver driver;
        public static string gridURL = "@hub.lambdatest.com/wd/hub";
        public static string LT_USERNAME = Environment.GetEnvironmentVariable("LT_USERNAME");
        public static string LT_ACCESS_KEY = Environment.GetEnvironmentVariable("LT_ACCESS_KEY");

        [SetUp]
        public void Setup()
        {
            var desiredCapabilities = new DesiredCapabilities();
            desiredCapabilities.SetCapability("browserName", "Chrome");
            desiredCapabilities.SetCapability("platform", "Windows 11");
            desiredCapabilities.SetCapability("version", "101.0");
            desiredCapabilities.SetCapability("screenResolution", "1280x800");
            desiredCapabilities.SetCapability("user", LT_USERNAME);
            desiredCapabilities.SetCapability("accessKey", LT_ACCESS_KEY);
            desiredCapabilities.SetCapability("build", "Selenium C-Sharp");
            desiredCapabilities.SetCapability("name", "Selenium Test");
            driver = new RemoteWebDriver(new Uri($"https://{LT_USERNAME}:{LT_ACCESS_KEY}{gridURL}"), desiredCapabilities, TimeSpan.FromSeconds(600));
        }

        [Test]
        [TestCase("Monday")]
        [TestCase("Tuesday")]
        [TestCase("Wednesday")]
        [TestCase("Thursday")]
        [TestCase("Friday")]
        [TestCase("Saturday")]
        [TestCase("Sunday")]
        [Parallelizable(ParallelScope.All)]
        public void ValidateDropDownSelection(string dayOfTheWeek)
        {
            driver.Navigate().GoToUrl("https://www.lambdatest.com/selenium-playground/select-dropdown-demo");
            var dropDown = new SelectElement(driver.FindElement(By.Id("select-demo")));
            dropDown.SelectByValue(dayOfTheWeek);
            string actualText = driver.FindElement(By.CssSelector(".selected-value.text-size-14")).Text;
            Assert.True(actualText.Contains(dayOfTheWeek), $"The expected day of the week {dayOfTheWeek} was not selected. The actual text was: {actualText}.");
        }

        [Test]
        public void ValidateMultipleSelection()
        {
            driver.Navigate().GoToUrl("https://www.lambdatest.com/selenium-playground/select-dropdown-demo");
            string[] selectedStates = { "Florida", "New York", "Texas" };
            var multiSelect = new SelectElement(driver.FindElement(By.Id("multi-select")));
            if (!multiSelect.IsMultiple)
            {
                Assert.Fail("The Select does not allow multiple selection.");
            }
            foreach (var state in selectedStates)
            {
                multiSelect.SelectByText(state);
            }
            multiSelect.DeselectAll();
        }

        [TearDown]
        public void TearDown()
        {
            Thread.Sleep(3000);
            driver.Quit();
        }
    }
}