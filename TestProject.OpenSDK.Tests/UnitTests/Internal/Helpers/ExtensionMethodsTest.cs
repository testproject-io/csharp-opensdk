using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;
using TestProject.OpenSDK.Internal.Helpers;

namespace TestProject.OpenSDK.Tests.UnitTests.Internal.Helpers
{
    [TestClass]
    public class ExtensionMethodsTest
    {
        [TestMethod]
        public void ShouldBePatched_UsingSendKeysToElementCommand_ShouldReturnTrue()
        {
            string driverCommand = DriverCommand.SendKeysToElement;

            Assert.IsTrue(driverCommand.ShouldBePatched());
        }

        [TestMethod]
        public void ShouldBePatched_UsingSendKeysToActiveCommand_ShouldReturnTrue()
        {
            string driverCommand = DriverCommand.SendKeysToActiveElement;

            Assert.IsTrue(driverCommand.ShouldBePatched());
        }

        [TestMethod]
        public void ShouldBePatched_UsingClickElementCommand_ShouldReturnFalse()
        {
            string driverCommand = DriverCommand.ClickElement;

            Assert.IsFalse(driverCommand.ShouldBePatched());
        }
    }
}
