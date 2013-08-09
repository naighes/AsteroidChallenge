using System;
using System.Threading;
using Microsoft.Xna.Framework.Input.Touch;

namespace DNT.Engine.Core.Messaging
{
    public class GestureMessageFactory
    {
        private GestureMessageFactory() { }

        public static GestureMessageFactory Instance
        {
            get
            {
                if (_instance.IsNotNull())
                    return _instance;

                Monitor.Enter(Lock);

                if (_instance.IsNull())
                {
                    var tmp = new GestureMessageFactory();
                    Interlocked.Exchange(ref _instance, tmp);
                }

                Monitor.Exit(Lock);

                return _instance;
            }
        }
        private static GestureMessageFactory _instance;

        private static readonly Object Lock = new Object();

        internal IMessage Create(GestureSample gestureSample)
        {
            switch (gestureSample.GestureType)
            {
                case GestureType.Tap:
                    return new Message<TapGesture>(new TapGesture(gestureSample));
            }

            return null;
        }
    }
}