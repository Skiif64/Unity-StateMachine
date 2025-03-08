using StateMachine.Abstractions;

namespace StateMachine
{
    public abstract class HierarchicalStateBase<TContext> : IState<TContext>, ITransitionable<TContext>
    {
        private bool _canExit = true;
        protected StateMachine<TContext> ChildStateMachine { get; }
        protected IState<TContext> InitialState { get; }
        protected TContext Context { get; }
        

        public bool CanExit
        {
            get => _canExit && ChildStateMachine.CurrentState.CanExit;
            protected set => _canExit = value;
        }

        protected HierarchicalStateBase(TContext context, IState<TContext> initialState)
        {
            ChildStateMachine = new StateMachine<TContext>();
            Context = context;
            InitialState = initialState;
        }

        public void AnyState(ITransition<TContext> transition)
        {
            ChildStateMachine.AnyState(transition);
        }
        
        public void FromState(IState<TContext> from, ITransition<TContext> transition)
        {
            ChildStateMachine.FromState(from, transition);
        }
       

        protected virtual void OnEnter()
        {
        }
        
        protected virtual void OnUpdate()
        {
        }
        
        protected virtual void OnFixedUpdate()
        {
        }
        
        protected virtual void OnExit()
        {
        }
        
        void IState<TContext>.OnEnter()
        {
            ChildStateMachine.Enter(InitialState);
            OnEnter();
        }

        void IState<TContext>.OnUpdate()
        {
            OnUpdate();
            ChildStateMachine.Update();
        }
        
        void IState<TContext>.OnFixedUpdate()
        {
            OnFixedUpdate();
            ChildStateMachine.FixedUpdate();
        }

        void IState<TContext>.OnExit()
        {
            OnExit();
            ChildStateMachine.Exit();
        }
    }

    public abstract class HierarchicalStateBase<TParent, TContext> : HierarchicalStateBase<TContext>,
        IState<TParent, TContext>
        where TParent : IState<TContext>
    {
        public TParent Parent { get; }
        
        protected HierarchicalStateBase(
            TContext context,
            TParent parent,
            IState<TContext> initialState) 
            : base(context, initialState)
        {
            Parent = parent;
        }

    }
}