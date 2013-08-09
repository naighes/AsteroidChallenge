using System;
using DNT.Engine.Core.Messaging;
using DNT.Engine.Core.Validation;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework.Input.Touch;

namespace DNT.Engine.Core.Input
{
    public class InputManager : IInputManager
    {
        public InputManager()
        {
            _accelerometer = new Accelerometer();
        }

        public void SubscribeTap(ISceneComponent subscriber, Action<Message<TapGesture>> action)
        {
            Verify.That(subscriber).Named("subscriber").IsNotNull();
            Messenger.Register<TapGesture>(subscriber, action);
        }

        public void SubscribeAccelerometer(ISceneComponent subscriber, Action<Message<AccelerometerReading>> action)
        {
            Verify.That(subscriber).Named("subscriber").IsNotNull();
            Messenger.Register<AccelerometerReading>(subscriber, action);
        }

        public void GesturesLookup()
        {
            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();
                var message = GestureMessageFactory.Instance.Create(gesture);

                if (message.IsNotNull())
                    Messenger.Send(message);
            }
        }

        public void StartAccelerometer()
        {
            _accelerometer.Start();
            _accelerometer.CurrentValueChanged += OnAccelerometerValueChanged;
        }

        private void OnAccelerometerValueChanged(Object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            Messenger.Send<AccelerometerReading>(new Message<AccelerometerReading>(e.SensorReading));
        }

        public void StopAccelerometer()
        {
            _accelerometer.Stop();
        }

        private readonly Accelerometer _accelerometer;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(Boolean disposing)
        {
            if (!disposing)
                return;

            if (_accelerometer.IsNull())
                return;

            try { _accelerometer.CurrentValueChanged -= OnAccelerometerValueChanged; }
            catch { }

            _accelerometer.Dispose();
        }

        ~InputManager()
        {
            Dispose(false);
        }
    }
}