using Harmony.Testing;
using Harmony.Unity;
using NSubstitute;
using NUnit.Framework;

namespace Harmony.EventSystem
{
    public class UnityUpdatableEventChannelTest : UnitTestCase
    {
        private UnityUpdatableEventChannel<EventForTest, UpdateForTest> eventChannel;

        [SetUp]
        public void Before()
        {
            eventChannel = CreateBehaviour<UpdatableEventChannelForTest>();
        }

        [Test]
        public void CanAskUpdateFromPublishers()
        {
            EventChannelUpdateHandler<UpdateForTest> handler = CreateSubstitute<EventChannelUpdateHandler<UpdateForTest>>();
            EventChannelUpdateRequestHandler<UpdateForTest> publisher = CreateSubstitute<EventChannelUpdateRequestHandler<UpdateForTest>>();

            eventChannel.OnUpdateRequested += publisher;
            eventChannel.RequestUpdate(handler);

            publisher.Received()(handler);
        }

        [Test]
        public void CanUnregisterFromPublishers()
        {
            EventChannelUpdateHandler<UpdateForTest> handler = CreateSubstitute<EventChannelUpdateHandler<UpdateForTest>>();
            EventChannelUpdateRequestHandler<UpdateForTest> publisher = CreateSubstitute<EventChannelUpdateRequestHandler<UpdateForTest>>();

            eventChannel.OnUpdateRequested += publisher;
            eventChannel.OnUpdateRequested -= publisher;
            eventChannel.RequestUpdate(handler);

            publisher.Received(0)(handler);
        }

        public class UpdatableEventChannelForTest : UnityUpdatableEventChannel<EventForTest, UpdateForTest>
        {  
        }

        public class EventForTest : IEvent
        {
        }

        public class UpdateForTest : IUpdate
        {
        }
    }
}