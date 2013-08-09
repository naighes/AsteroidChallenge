using System;
using DNT.Engine.Core.Messaging;
using Microsoft.Devices.Sensors;

namespace DNT.Engine.Core.Input
{
    public interface IInputManager : IDisposable
    {
        void SubscribeTap(ISceneComponent subscriber, Action<Message<TapGesture>> action);
        void GesturesLookup();
        void StartAccelerometer();
        void StopAccelerometer();
        void SubscribeAccelerometer(ISceneComponent subscriber, Action<Message<AccelerometerReading>> action);
    }
}