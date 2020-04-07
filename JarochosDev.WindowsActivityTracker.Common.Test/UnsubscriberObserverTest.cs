using System;
using System.Collections.Generic;
using JarochosDev.WindowsActivityTracker.Common.Models;
using Moq;
using NUnit.Framework;

namespace JarochosDev.WindowsActivityTracker.Common.Test
{
    public class UnsubscriberObserverTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Constructor()
        {
            var observers = new List<IObserver<IWindowsSystemEvent>>()
            {
                new Mock<IObserver<IWindowsSystemEvent>>().Object
            };

            var mockFakeObserverToUnsubscribe = new Mock<IObserver<IWindowsSystemEvent>>();
            var unsubscriberObserver = new UnsubscriberObserver<IWindowsSystemEvent>(observers, mockFakeObserverToUnsubscribe.Object );
            CollectionAssert.AreEqual(observers, unsubscriberObserver.ReadOnlyObservers);
            Assert.AreSame(mockFakeObserverToUnsubscribe.Object, unsubscriberObserver.ObserverToUnsubscribe);
        }

        [Test]
        public void Test_Dispose_Remove_ObserverFromList()
        {
            var mockObserverInList = new Mock<IObserver<IWindowsSystemEvent>>();

            var observers = new List<IObserver<IWindowsSystemEvent>>()
            {
                mockObserverInList.Object
            };

            var unsubscriberObserver = new UnsubscriberObserver<IWindowsSystemEvent>(observers, mockObserverInList.Object);

            unsubscriberObserver.Dispose();

            Assert.AreEqual(0, unsubscriberObserver.ReadOnlyObservers.Count);
        }
    }
}