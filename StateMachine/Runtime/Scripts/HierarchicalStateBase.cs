using System;
using System.Collections.Generic;
using StateMachine;
using StateMachine.Abstractions;

namespace StateMachine
{
    public abstract class HierarchicalStateBase<TContext> : IState<TContext>
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

        public void AddTransition(Transition<TContext> transition)
        {
            ChildStateMachine.AddTransition(transition);
        }
       

        protected virtual void OnEnter()
        {
        }
        
        protected virtual void OnUpdate()
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

        void IState<TContext>.OnExit()
        {
            OnExit();
            ChildStateMachine.Exit();
        }
    }
}