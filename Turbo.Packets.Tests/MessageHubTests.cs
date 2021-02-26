using Microsoft.Extensions.Logging.Abstractions;
using System;
using Turbo.Core.Players;
using Turbo.Packets.Composers;
using Turbo.Packets.Incoming;
using Turbo.Packets.Sessions;
using Xunit;

namespace Turbo.Packets.Tests
{
    public class MessageHubTests
    {
        private readonly IPacketMessageHub _hub;
        private readonly object _subscriber;
        private object _subscriber2;
        private readonly ISession _mockSession;

        public MessageHubTests()
        {
            this._hub = new PacketMessageHub(new NullLogger<PacketMessageHub>());
            this._subscriber = new object();
            this._subscriber2 = new object();
            this._mockSession = new MockSession();
        }

        [Fact]
        public void Publish_CallsAllRegisteredActions()
        {
            int callCount = 0;
            _hub.Subscribe(new object(), new Action<MockEvent, ISession>( (a, b) => callCount++));
            _hub.Subscribe(new object(), new Action<MockEvent, ISession>( (a, b) => callCount++));

            
            _hub.Publish(new MockEvent(), this._mockSession);

            Assert.Equal(2, callCount);
        }

        [Fact]
        public void Publish_SpecialEvent_CaughtByBase()
        {
            int callCount = 0;
            _hub.Subscribe<MockEvent>(_subscriber, (a, b) => callCount++);
            _hub.Subscribe(_subscriber, new Action<MockEvent, ISession>( (a,b) => callCount++));

            _hub.Publish(new MockEvent2("data"), this._mockSession);

            Assert.Equal(2, callCount);
        }

        [Fact]
        public void Publish_BaseEvent_NotCaughtBySpecial()
        {
            int callCount = 0;
            _hub.Subscribe(_subscriber, new Action<MockEvent2, ISession>( (a, b) => callCount++));
            _hub.Subscribe(_subscriber, new Action<MockEvent, ISession>( (a, b) => callCount++));

            _hub.Publish(new MockEvent(), this._mockSession);

            Assert.Equal(1, callCount);
        }

        [Fact]
        public void Cancel_Event_WithCallable()
        {
            int callCount = 0;
            ICallable<MockEvent> callable = new MockCallableCancels();
            _hub.Subscribe(_subscriber, new Action<MockEvent, ISession>((a, b) => callCount++));
            _hub.RegisterCallable(callable);

            Assert.Single(_hub.GetCallables<MockEvent>()); // we have registered a callable

            _hub.Publish(new MockEvent(), this._mockSession);
            _hub.PublishAsync(new MockEvent(), this._mockSession);

            Assert.Equal(0, callCount);// event was cancelled, should not call listener

            _hub.UnRegisterCallable(callable);

            _hub.Publish(new MockEvent(), this._mockSession);

            Assert.Equal(1, callCount); // event was not cancelled, should call listener
        }

        [Fact]
        public void Cancel_Event_OfTypeOnly()
        {
            int callCount = 0;
            ICallable<MockEvent> callable = new MockCallableCancels();
            _hub.Subscribe(_subscriber, new Action<MockEvent2, ISession>((a, b) => callCount++));
            _hub.RegisterCallable(callable);

            Assert.Single(_hub.GetCallables<MockEvent>()); // we have registered a callable for that type
            Assert.Empty(_hub.GetCallables<MockEvent2>()); // we dont have any callables for that event

            _hub.Publish(new MockEvent2("data"), this._mockSession);

            Assert.Equal(1, callCount);// our type of event was not cancelled, should call listener

            _hub.UnRegisterCallable(callable);
        }

        [Fact]
        public void Unsubscribe_RemovesAllHandlers_OfAnyType_ForSender()
        {
            bool subscr2 = false;
            bool subscr1 = false;

            _hub.Subscribe(_subscriber2, new Action<MockEvent, ISession>( (a, b) => { subscr2 = true; }));
            _hub.Subscribe(_subscriber, new Action<MockEvent, ISession>( (a,b) => { subscr1 = true; }));
            _hub.Subscribe(_subscriber, new Action<MockEvent2, ISession>((a, b) => { subscr1 = true; }));
            _hub.Unsubscribe(_subscriber);

            Assert.True(_hub.Exists(_subscriber2));
            Assert.False(_hub.Exists(_subscriber));

            _hub.Publish(new MockEvent2("data"), this._mockSession);
            _hub.Publish(new MockEvent(), this._mockSession);

            Assert.False(subscr1);
            Assert.True(subscr2);
        }

        [Fact]
        public void Unsubscribe_RemovesAllHandlers_OfSpecificType_ForSender()
        {
            _hub.Subscribe(_subscriber, new Action<MockEvent, ISession>( (a, b) => { }));
            _hub.Subscribe(_subscriber, new Action<MockEvent2, ISession>( (a, b) => { }));
            _hub.Subscribe(_subscriber2, new Action<MockEvent, ISession>( (a, b) => { }));

            _hub.Unsubscribe<MockEvent>(_subscriber);

            Assert.False(_hub.Exists<MockEvent>(_subscriber));
            Assert.True(_hub.Exists<MockEvent>(_subscriber2));
            Assert.True(_hub.Exists<MockEvent2>(_subscriber));
        }

        [Fact]
        public void Unsubscribe_RemovesSpecificHandler_ForSender()
        {
            Action<MockEvent, ISession> actionToDie = new Action<MockEvent, ISession>( (a,b) => { });
            _hub.Subscribe(_subscriber, actionToDie);
            _hub.Subscribe(_subscriber, new Action<MockEvent, ISession>( (a, b) => { }));
            _hub.Subscribe(_subscriber2, new Action<MockEvent, ISession>( (a, b) => { }));

            _hub.Unsubscribe(_subscriber, actionToDie);

            Assert.False(_hub.Exists(_subscriber, actionToDie));
            Assert.True(_hub.Exists<MockEvent>(_subscriber));
            Assert.True(_hub.Exists<MockEvent>(_subscriber2));
        }

        [Fact]
        public void Exists_EventDoesExist()
        {
            var action = new Action<MockEvent, ISession>( (a, b) => { });

            _hub.Subscribe(_subscriber, action);

            Assert.True(_hub.Exists(_subscriber, action));
        }

        [Fact]
        public void Unsubscribe_CleanUps()
        {
            _hub.Subscribe(_subscriber, new Action<MockEvent, ISession>( (a, b) => { }));
            _hub.Subscribe(_subscriber2, new Action<MockEvent, ISession>( (a, b) => { }));

            _subscriber2 = null;

            GC.Collect();

            _hub.Unsubscribe<MockEvent>(_subscriber);

            Assert.False(_hub.Exists(_subscriber2));
            Assert.False(_hub.Exists(_subscriber));
        }

        [Fact]
        public void Publish_NoExceptionRaisedWhenHandlerCreatesNewSubscriber()
        {
            _hub.Subscribe(_subscriber, new Action<MockEvent, ISession>( (a, b) => new MockListener(_hub)));

            try
            {
                _hub.Publish(new MockEvent(), _mockSession);
            }

            catch (InvalidOperationException e)
            {
                Assert.True(false, $"Expected no exception, but got: {e}");
            }
        }

        internal class MockEvent : IMessageEvent
        {

        }

        internal class MockEvent2 : MockEvent
        {
            public string Data { get; }

            public MockEvent2(string data)
            {
                this.Data = data;
            }
        }

        internal class MockCallableCancels : ICallable<MockEvent>
        {
            public bool Call(MockEvent message, ISession session)
            {
                return false;
            }
        }

        internal class MockSession : ISession
        {
            public IPlayer Player { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public string IPAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public string Revision { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public void Disconnect()
            {
                throw new NotImplementedException();
            }

            public ISession Send(IComposer composer)
            {
                throw new NotImplementedException();
            }

            public ISession SendQueue(IComposer composer)
            {
                throw new NotImplementedException();
            }
        }

        internal class MockListener
        {
            public MockListener(IPacketMessageHub hub)
            {
                hub.Subscribe<MockEvent>(this, this.MockListenerFunc);
            }

            public void MockListenerFunc(MockEvent msg, ISession session)
            {

            }
        }
    }
}
