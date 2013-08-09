using System;
using Microsoft.Xna.Framework;

namespace DNT.Engine.Core.Animations
{
    public class AuditableTransition : Transition, IAuditableTransition
    {
        internal AuditableTransition(Single initialValue,
                                     Single finalValue,
                                     TimeSpan time,
                                     Func<Single, Single, Single, Single> interpolationFunc)
            : base(initialValue, finalValue, time, interpolationFunc)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (IsCompleted)
                return;

            UpdateCurrentTransitionAmount(gameTime);

            if (!HasReachedFinalValue())
                return;

            SetAsCompleted();
        }

        private void SetAsCompleted()
        {
            IsCompleted = true;
            OnCompleted(EventArgs.Empty);
        }

        public event EventHandler Completed;

        private void OnCompleted(EventArgs e)
        {
            var handler = Completed;

            if (handler.IsNotNull())
                handler(this, e);
        }
    }
}