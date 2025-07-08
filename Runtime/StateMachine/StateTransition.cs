using System;

namespace Amenity.StateMachine
{
    public readonly struct StateTransition<TState, TEvent>
        where TState : Enum
        where TEvent : Enum
    {
        private readonly TState currentState;
        private readonly TEvent battleEvent;

        public StateTransition(TState currentState, TEvent battleEvent)
        {
            this.currentState = currentState;
            this.battleEvent = battleEvent;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * this.currentState.GetHashCode() + 31 * this.battleEvent.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }
            var other = (StateTransition<TState, TEvent>)obj;
            return Convert.ToInt32(this.currentState) == Convert.ToInt32(other.currentState) &&
                   Convert.ToInt32(this.battleEvent) == Convert.ToInt32(other.battleEvent);
        }
    }
}