using System.Collections.Concurrent;
using System.Collections.Generic;
using StateMachine.Abstractions;

namespace StateMachine
{
    public class StateMachine<TContext>
    {
        private readonly ConcurrentDictionary<IState<TContext>, List<Transition<TContext>>> _transitions = new();
        
        public IState<TContext> CurrentState { get; private set; } = StateBase<TContext>.Null;

        public void Init(IState<TContext> initialState)
        {
            Enter(initialState);
        }
        public void Update()
        {
            TryTransitions();
            CurrentState.OnUpdate();
        }
        
        public void SwitchState(IState<TContext> newState)
        {
            CurrentState.OnExit();
            CurrentState = newState;
            newState.OnEnter();
        }

        public void AddTransition(Transition<TContext> transition)
        {
            _transitions.AddOrUpdate(transition.TargetState,
                _ => new List<Transition<TContext>>() { transition },
                (_, transitions) =>
                {
                    transitions.Add(transition);
                    return transitions;
                });
        }
        
        internal void Enter(IState<TContext> state)
        {
            SwitchState(state);
        }

        internal void Exit()
        {
            CurrentState.OnExit();
            CurrentState = StateBase<TContext>.Null;
        }

        private void TryTransitions()
        {
            if (!CurrentState.CanExit) return;
            
            foreach (var transition in _transitions[CurrentState])
            {
                if (!transition.IsSatisfied(this)) continue;
                
                SwitchState(transition.TransitionTo);
                break;
            }
        }
    }
}
