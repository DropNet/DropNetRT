using DropNetRT.Extensions;
using NUnit.Framework;

namespace DropNetRTTests
{
    [TestFixture]
    public class StringExtensionTests
    {
        [Test]
        public void EnsurePathGetsLeadingSlashRemoved()
        {
            string path = "/someDir";

            path = path.CleanPath();

            Assert.AreEqual("someDir", path);
        }

        [Test]
        public void CanHandleEmptyString()
        {
            string path = "";

            path = path.CleanPath();

            Assert.AreEqual("", path);
        }

        [Test]
        public void CanHandleNoSlash()
        {
            string path = "SomeDir";

            path = path.CleanPath();

            Assert.AreEqual("SomeDir", path);
        }
    }
}