using System;
using StateMachine.Abstractions;

namespace StateMachine.Transitions
{
    public readonly partial struct Transition<TContext>
    {
        public Transition<TContext> Create(IState<TContext> to, Func<bool> condition)
            => new Transition<TContext>(to, condition);
    }
}