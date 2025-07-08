using System;
using System.Collections.Generic;

namespace Amenity.StateMachine
{
    public class StateMachine<TState, TEvent>
        where TState : Enum
        where TEvent : Enum
    {
        public TState CurrentState => this.currentState;
        
        private TState currentState;

        private readonly Dictionary<StateTransition<TState, TEvent>, TransitionResult<TState>>
            transitions = new();

        public StateMachine(TState currentState)
        {
            this.currentState = currentState;
        }

        public void ProcessInputEvent(TEvent inputEvent)
        {
            this.currentState = GetNext(inputEvent);
        }

        public void AddTransition(TState from, TEvent byEvent, TState to, Action transitionAction)
        {
            var transitionKey = new StateTransition<TState, TEvent>(from, byEvent);
            var transitionResult = new TransitionResult<TState>(to, transitionAction);
            this.transitions[transitionKey] = transitionResult;
        }

        private TState GetNext(TEvent inputEvent)
        {
            var transition = new StateTransition<TState, TEvent>(this.currentState, inputEvent);
            if (!this.transitions.TryGetValue(transition, out var transitionResult)) {
                throw new Exception("Invalid transition: " + this.currentState + " -> " + inputEvent);
            }

            transitionResult.Callback?.Invoke();
            return transitionResult.NextState;
        }
    }
}