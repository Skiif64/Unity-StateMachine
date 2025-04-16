using StateMachine.Abstractions;

namespace StateMachine
{
    public abstract partial class StateBase<TContext> : IState<TContext>
    {
        protected TContext Context { get; }

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
        
        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual bool CanExit() => true;
    }

    public abstract class StateBase<TParent, TContext> : StateBase<TContext>, IState<TParent, TContext>
        where TParent : IState<TContext>
    {
        public TParent Parent { get; }
        
        protected StateBase(TParent parent, TContext context) : base(context)
        {
            Parent = parent;
        }

    }
}