using System;
using StateMachine.Abstractions;

namespace StateMachine.Transitions
{
    public readonly struct Transition<TContext> : ITransition<TContext>
    {
        public IState<TContext> TransitionTo { get; }
        private readonly Func<bool> _condition;

        public Transition(IState<TContext> transitionTo, Func<bool> condition)
        {
            TransitionTo = transitionTo;
            _condition = condition;
        }
        
        public bool IsSatisfied(IStateMachine<TContext> stateMachine)
        {
            return _condition();
        }
    }
}