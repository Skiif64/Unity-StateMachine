using System;

namespace StateMachine.Abstractions
{
    public interface IStateMachineTicker
    {
        IDisposable Subscribe<TContext>(IStateMachine<TContext> stateMachine);
    }
}