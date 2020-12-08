using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TestProject.OpenSDK.Drivers;
using TestProject.OpenSDK.Drivers.Web;

namespace TestProject.OpenSDK.Tests.Examples.Drivers
{
    [TestClass]
    public class ChromeDriverTest
    {
        private ChromeDriver driver;

        [TestInitialize]
        public void StartBrowser()
        {
            OpenQA.Selenium.Chrome.ChromeOptions chromeOptions = new OpenQA.Selenium.Chrome.ChromeOptions();
            chromeOptions.PageLoadStrategy = PageLoadStrategy.Normal;
            chromeOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify;

            driver = new ChromeDriver(
                chromeOptions: chromeOptions,
                projectName: "My project",
                jobName: "My job",
                token: "aqqm_o3T_egvYLkI1eum8LV10IsHu-tKO3cRbJP6qW81");
        }

        [TestMethod]
        public void ExampleTestUsingChromeDriver()
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
