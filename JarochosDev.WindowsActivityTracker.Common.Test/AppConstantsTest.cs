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

        [Test]
        public void Test_Windows_Activity_Tracker_Connection_String()
        {
            Assert.AreEqual("WindowsActivityTrackerConnectionString", AppConstants.WINDOWS_ACTIVITY_TRACKER_CONNECTION_STRING);
        }

        public static string WINDOWS_ACTIVITY_TRACKER_CONNECTION_STRING = "WindowsActivityTrackerConnectionString";

    }
}

