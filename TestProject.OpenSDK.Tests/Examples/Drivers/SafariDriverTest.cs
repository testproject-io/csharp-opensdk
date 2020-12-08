using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TestProject.OpenSDK.Drivers.Web;

namespace TestProject.OpenSDK.Tests.Examples.Drivers
{
    [TestClass]
    public class SafariDriverTest
    {
        private SafariDriver driver;

        [TestInitialize]
        public void StartBrowser()
        {
            OpenQA.Selenium.Safari.SafariOptions safariOptions = new OpenQA.Selenium.Safari.SafariOptions();
            safariOptions.PageLoadStrategy = PageLoadStrategy.Normal;
            safariOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify;

            driver = new SafariDriver(
                safariOptions: safariOptions,
                projectName: "My project",
                jobName: "My job",
                token: "aqqm_o3T_egvYLkI1eum8LV10IsHu-tKO3cRbJP6qW81");
        }

        [TestMethod]
        public void ExampleTestUsingSafariDriver()
        {
            driver.Navigate().GoToUrl("https://example.testproject.io");
            driver.FindElement(By.CssSelector("#name")).SendKeys("John Smith");
            driver.FindElement(By.CssSelector("#password")).SendKeys("12345");
            driver.FindElement(By.CssSelector("#login")).Click();

            Assert.IsTrue(driver.FindElement(By.CssSelector("#greetings")).Displayed);
        }

        [TestCleanup]
        public void CloseBrowser()
        {
            driver.Quit();
        }
    }
}
