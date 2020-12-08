using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProject.OpenSDK.Internal.Helpers;

namespace TestProject.OpenSDK.Tests.UnitTests.Internal.Helpers
{
    [TestClass]
    public class CommandHelperTest
    {
        [TestMethod]
        public void UpdateSendKeysParameters_NoValue_ShouldReturnUnchangedParameters()
        {
            Dictionary<string, object> originalParams = new Dictionary<string, object>();
            originalParams.Add("abc", "def");

            Dictionary<string, object> modifiedParams = CommandHelper.UpdateSendKeysParameters(originalParams);

            Assert.AreEqual(originalParams, modifiedParams);
        }

        [TestMethod]
        public void UpdateSendKeysParameters_WithValueButNotAnArray_ShouldReturnUnchangedParameters()
        {
            Dictionary<string, object> originalParams = new Dictionary<string, object>();
            originalParams.Add("value", "abc");

            Dictionary<string, object> modifiedParams = CommandHelper.UpdateSendKeysParameters(originalParams);

            Assert.AreEqual(originalParams, modifiedParams);
        }

        [TestMethod]
        public void UpdateSendKeysParameters_WithValueThatIsAnArray_ShouldReturnPatchedParameters()
        {
            Dictionary<string, object> originalParams = new Dictionary<string, object>();
            originalParams.Add("value", new string[] { "astring" });

            Dictionary<string, object> modifiedParams = CommandHelper.UpdateSendKeysParameters(originalParams);

            Assert.IsTrue(modifiedParams.ContainsKey("text"));
            Assert.AreEqual("astring", modifiedParams["text"]);
            Assert.IsFalse(modifiedParams.ContainsKey("value"));
        }
    }
}
