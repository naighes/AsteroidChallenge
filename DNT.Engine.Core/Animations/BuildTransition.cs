using System;

namespace DNT.Engine.Core.Animations
{
    public static class BuildTransition
    {
        public static ITransitionInfo Between(Single initialValue, Single finalValue)
        {
            return new TransitionInfo(initialValue, finalValue);
        }
    }
}