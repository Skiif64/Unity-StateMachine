using System;
using StateMachine.Abstractions;

namespace StateMachine
{
    public class NullStateMachineTicker : IStateMachineTicker
    {
        public static NullStateMachineTicker Instance { get; } = new NullStateMachineTicker();
        
        public IDisposable Subscribe<TContext>(IStateMachine<TContext> stateMachine)
        {
            return EmptyDisposable.Instance;
        }

        private readonly struct EmptyDisposable : IDisposable
        {
            public static EmptyDisposable Instance { get; } = new EmptyDisposable();
            
            public void Dispose()
            {
                //noop
            }

        }
    }
}