using System;
using StateMachine.Abstractions;

namespace StateMachine
{
    public readonly struct Transition<TContext>
    {
        public IState<TContext> TargetState { get; }
        public IState<TContext> TransitionTo { get; }
        private readonly Func<bool> _condition;

        public Transition(IState<TContext> targetState, IState<TContext> transitionTo, Func<bool> condition)
        {
            TargetState = targetState;
            TransitionTo = transitionTo;
            _condition = condition;
        }
        
        public bool IsSatisfied(StateMachine<TContext> stateMachine)
        {
            if (stateMachine.CurrentState != TargetState)
                return false;

            return _condition();
        }
    }
}