using System;

namespace DNT.Engine.Core.Messaging
{
    public interface IMessage
    {
        Object Content { get; }
    }
}