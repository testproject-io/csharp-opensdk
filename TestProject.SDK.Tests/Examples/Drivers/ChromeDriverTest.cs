using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProject.SDK.Drivers.Web;

namespace TestProject.SDK.Tests.Examples.Drivers
{
    [TestClass]
    public class ChromeDriverTest
    {
        [TestMethod]
        public void ExampleTestUsingChromeDriver()
        {
            ChromeDriver driver = new ChromeDriver(
                projectName: "My project",
                jobName: "My job",
                token: "aqqm_o3T_egvYLkI1eum8LV10IsHu-tKO3cRbJP6qW81");

            driver.Quit();
        }
    }
}
