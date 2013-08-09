using System;
using System.Collections.Generic;
using DNT.Engine.Core.Validation;

namespace DNT.Engine.Core.Messaging
{
    public static class Messenger
    {
        static Messenger()
        {
            _subscriptions = new Dictionary<Type, IDictionary<Object, Action<IMessage>>>();
        }

        public static void Register<T>(Object subscriber, Action<Message<T>> action)
        {
            Verify.That(subscriber).Named("subscriber").IsNotNull();

            if (!_subscriptions.ContainsKey(typeof(T)))
                _subscriptions.Add(typeof(T), new Dictionary<Object, Action<IMessage>>());

            _subscriptions[typeof(T)].Add(subscriber, o => action(o as Message<T>));
        }

        public static void Send<T>(Message<T> message)
        {
            Send((IMessage)message);
        }

        public static void Send(IMessage message)
        {
            if (_subscriptions.ContainsKey(message.Content.GetType()))
                foreach (var subscriber in _subscriptions[message.Content.GetType()])
                    if (subscriber.Value.IsNotNull())
                        subscriber.Value(message);
        }

        private static readonly IDictionary<Type, IDictionary<Object, Action<IMessage>>> _subscriptions;
    }
}