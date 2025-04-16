using System;
using StateMachine.Abstractions;

namespace StateMachine
{
    public abstract class HierarchicalStateBase<TContext> : IState<TContext>, IStateMachine<TContext>, IDisposable
    {
        protected StateMachine<TContext> ChildStateMachine { get; }
        protected IState<TContext> InitialState { get; }
        protected TContext Context { get; }

        public IState<TContext> CurrentState => ChildStateMachine.CurrentState;

        protected HierarchicalStateBase(TContext context, IState<TContext> initialState, IStateMachineTicker ticker)
        {
            ChildStateMachine = new StateMachine<TContext>(ticker);
            Context = context;
            InitialState = initialState;
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
        
        public void Init(IState<TContext> initialState) => ChildStateMachine.Init(initialState);

        public void Update() => ChildStateMachine.Update();

        public void FixedUpdate() => ChildStateMachine.FixedUpdate();

        public void CheckTransitions() => ChildStateMachine.CheckTransitions();

        public void SwitchState(IState<TContext> newState) => ChildStateMachine.SwitchState(newState);

        public virtual bool CanExit() => true;

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
            Update();
        }
        
        void IState<TContext>.OnFixedUpdate()
        {
            OnFixedUpdate();
            FixedUpdate();
        }

        void IState<TContext>.OnExit()
        {
            OnExit();
            ChildStateMachine.Exit();
        }

        bool IState<TContext>.CanExit() => CanExit();

        public void Dispose()
        {
            ChildStateMachine?.Dispose();
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
}