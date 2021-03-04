using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Packets.Incoming;
using Turbo.Packets.Sessions;

namespace Turbo.Packets
{
    public interface IPacketMessageHub
    {
        public void Publish<T>(T message, ISession session) where T : IMessageEvent;
        public Task PublishAsync<T>(T message, ISession session) where T : IMessageEvent;
        public void Subscribe<T>(object subscriber, Action<T, ISession> handler) where T : IMessageEvent;
        public void Subscribe<T>(object subscriber, Func<T, ISession, Task> handler) where T : IMessageEvent;
        public void RegisterCallable<T>(ICallable<T> callable) where T : IMessageEvent;
        public void UnRegisterCallable<T>(ICallable<T> callable) where T : IMessageEvent;
        public void Unsubscribe(object subscriber);
        public void Unsubscribe<T>(object subscriber, Action<T, ISession> handler = null) where T : IMessageEvent;
        public void Unsubscribe<T>(object subscriber, Func<T, ISession, Task> handler) where T : IMessageEvent;
        public bool Exists(object subscriber);
        public bool Exists<T>(object subscriber) where T : IMessageEvent;
        public bool Exists<T>(object subscriber, Action<T, ISession> handler) where T : IMessageEvent;
        public bool Exists<T>(object subscriber, Func<T, ISession, Task> handler) where T : IMessageEvent;
        public List<ICallable<T>> GetCallables<T>() where T : IMessageEvent;
    }
}
