using JarochosDev.TestUtilities.Net.NetStandard;
using JarochosDev.WindowsActivityTracker.Common;
using JarochosDev.WindowsActivityTracker.WindowsService.FormObjects;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.WindowsService.Test.FormObjects
{
    class WindowsSystemEventListenerFormTest
    {
        private Mock<IWindowsSystemEventServiceManager> _mockWindowsSystemEventServiceManager;
        private WindowsSystemEventListenerForm _windowsSystemEventListenerForm;

        [SetUp]
        public void SetUp()
        {
            _mockWindowsSystemEventServiceManager = new Mock<IWindowsSystemEventServiceManager>();
            _windowsSystemEventListenerForm = new WindowsSystemEventListenerForm(_mockWindowsSystemEventServiceManager.Object);
        }

        [Test]
        public void Test_Constructor()
        {
            Assert.AreSame(_mockWindowsSystemEventServiceManager.Object, _windowsSystemEventListenerForm.WindowsSystemEventServiceManager);
        }

        [Test]
        public void IsOfTypeHidedForm()
        {
            Assert.IsInstanceOf<HidedForm>(_windowsSystemEventListenerForm);
        }

        [Test]
        public void Test_OnLoad_Calls_WindowsEventServiceManager_Start()
        {
            UnitTestUtilities.Helper.RunProtectedMethod(_windowsSystemEventListenerForm, "OnLoadForm");
            _mockWindowsSystemEventServiceManager.Verify(s=>s.Start(), Times.Once);
        }

        [Test]
        public void Test_OnClosing_Calls_WindowsEventServiceManager_Stop()
        {
            UnitTestUtilities.Helper.RunProtectedMethod(_windowsSystemEventListenerForm, "OnCloseForm");
            _mockWindowsSystemEventServiceManager.Verify(s => s.Start(), Times.Once);
        }

        

    }
}
