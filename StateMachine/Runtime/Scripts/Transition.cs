using System;
using StateMachine.Abstractions;

namespace StateMachine
{
    public readonly struct Transition<TContext>
    {
        public IState<TContext> TransitionTo { get; }
        private readonly Func<bool> _condition;

        public Transition(IState<TContext> transitionTo, Func<bool> condition)
        {
            TransitionTo = transitionTo;
            _condition = condition;
        }
        
        public bool IsSatisfied(StateMachine<TContext> stateMachine)
        {
            return _condition();
        }
    }
}