using System;
using StateMachine.Abstractions;

namespace StateMachine.Transitions
{
    public readonly struct Transition
    {
        public static Transition<TContext> Create<TContext>(IState<TContext> to, Func<bool> condition)
            => new Transition<TContext>(to, condition);
    }
}