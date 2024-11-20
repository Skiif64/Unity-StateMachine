using System;
using StateMachine.Abstractions;

namespace StateMachine
{
    public abstract partial class StateBase<TContext> : IState<TContext>
    {
        protected TContext Context { get; }
        public bool CanExit { get; protected set; } = true;

        protected StateBase(TContext context)
        {
            Context = context;
        }
        public virtual void OnEnter()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}