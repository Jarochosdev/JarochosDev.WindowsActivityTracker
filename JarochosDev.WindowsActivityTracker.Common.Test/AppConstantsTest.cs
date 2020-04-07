using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Common.Test
{
    class AppConstantsTest
    {
        [Test]
        public void Test_Logging_Application_Name()
        {
            Assert.AreEqual("Windows Activity Tracker", AppConstants.LOGGING_APPLICATION_NAME);
        }
    }
}
