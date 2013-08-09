using System;
using DNT.Engine.Core.Validation;

namespace DNT.Engine.Core.Messaging
{
    public class Message<T> : IMessage
    {
        public Message(T content)
        {
            Verify.That(content).Named("content").IsNotNull();
            _content = content;
        }

        private readonly T _content;

        public T Content
        {
            get { return _content; }
        }

        Object IMessage.Content
        {
            get { return _content; }
        }
    }
}