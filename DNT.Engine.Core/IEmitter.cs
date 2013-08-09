using System;

namespace DNT.Engine.Core
{
    public interface IEmitter : ISceneComponent
    {
        void Emit(Int32 particlesToEmit);
        void Start();
        void Stop();
    }
}