using Harmony.Testing;
using Harmony.Unity;
using NSubstitute;
using NUnit.Framework;

namespace Harmony.EventSystem
{
    public class UnityEventChannelTest : UnitTestCase
    {
        private UnityEventChannel<EventForTest> eventChannel;

        [SetUp]
        public void Before()
        {
            eventChannel = CreateBehaviour<EventChannelForTest>();
        }

        [Test]
        public void CanRegisterToChannelAndReceiveEvents()
        {
            EventChannelHandler<EventForTest> channelHandler = CreateSubstitute<EventChannelHandler<EventForTest>>();
            eventChannel.OnEventPublished += channelHandler;

            EventForTest eventForTest = new EventForTest();
            eventChannel.Publish(eventForTest);

            channelHandler.Received()(eventForTest);
        }

        public class EventChannelForTest : UnityEventChannel<EventForTest>
        {
            
        }

        public class EventForTest : IEvent
        {
        }
    }
}