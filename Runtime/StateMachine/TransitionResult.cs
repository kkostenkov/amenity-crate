using System;

namespace Amenity.StateMachine
{
    public class TransitionResult<TState> where TState : Enum
    {
        public readonly TState NextState;
        public readonly Action Callback;

        public TransitionResult(TState nextState, Action callback)
        {
            this.NextState = nextState;
            this.Callback = callback;
        }
    }
}