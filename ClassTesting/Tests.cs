using System;
using NUnit.Framework;

namespace ClassTesting
{
    [TestFixture]
    public class Tests
    {
        [Test(ExpectedResult = true)]
        public bool TestVideoDeviceFind()
        {
            var webCamManager = new WebCamManager();
            return webCamManager.IsHaveDivices();
        }
    }
}