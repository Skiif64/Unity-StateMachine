using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using StateMachine.Abstractions;

namespace StateMachine
{
    public class StateMachine<TContext> : IStateMachine<TContext>, IDisposable
    {
        private readonly HashSet<ITransition<TContext>> _anyTransition = new();
        private readonly ConcurrentDictionary<IState<TContext>, List<ITransition<TContext>>> _transitions = new();
        private readonly ConcurrentDictionary<Type, IState<TContext>> _states = new();
        private readonly IStateMachineTicker _ticker;
        private IDisposable _updateSubscription;
        
        public IState<TContext> CurrentState { get; private set; } = StateBase<TContext>.Null;

        public StateMachine(IStateMachineTicker ticker)
        {
            _ticker = ticker;
        }

        public StateMachine() : this(NullStateMachineTicker.Instance)
        {
            
        }

        public void Init(IState<TContext> initialState)
        {
            _updateSubscription = _ticker.Subscribe(this);
            Enter(initialState);
        }
        public void Update()
        {
            CurrentState.OnUpdate();
        }
        
        public void FixedUpdate()
        {
            CurrentState.OnFixedUpdate();
        }

        public void CheckTransitions()
        {
            TryTransitions();
        }
        
        public void SwitchState(IState<TContext> newState)
        {
            if (newState.Equals(CurrentState))
            {
                return;
            }
            
            CurrentState.OnExit();
            CurrentState = newState;
            newState.OnEnter();
        }

        public TState GetStateOrDefault<TState>() where TState : IState<TContext>
        {
            if (!_states.TryGetValue(typeof(TState), out var state))
            {
                return default;
            }

            return (TState)state;
        }

        public void FromState(IState<TContext> from, ITransition<TContext> transition)
        {
            _states.TryAdd(from.GetType(), from);
            _states.TryAdd(transition.TransitionTo.GetType(), transition.TransitionTo);
            _transitions.AddOrUpdate(from,
                _ => new List<ITransition<TContext>>() { transition },
                (_, transitions) =>
                {
                    transitions.Add(transition);
                    return transitions;
                });
        }

        public void AnyState(ITransition<TContext> transition)
        {
            _states.TryAdd(transition.TransitionTo.GetType(), transition.TransitionTo);
            _anyTransition.Add(transition);
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
            foreach (var transition in _anyTransition)
            {
                if (CurrentState is IStateMachine<TContext> innerMachine) //TODO: use root ticker instead of type check
                {
                    innerMachine.CheckTransitions();
                }
                
                if (CurrentState.Equals(transition.TransitionTo))
                {
                    continue;    
                }

                if (!transition.IsSatisfied(this))
                {
                    continue;
                }

                if (!CurrentState.CanExit())
                {
                    return;
                }
                
                SwitchState(transition.TransitionTo);
                break;
            }

            if (!_transitions.TryGetValue(CurrentState, out var currentTransitions))
            {
                return; //TODO: log this case?
            }
            
            foreach (var transition in currentTransitions)
            {
                if (CurrentState is IStateMachine<TContext> innerMachine) //TODO: use root ticker instead of type check
                {
                    innerMachine.CheckTransitions();
                }
                
                if (!CurrentState.CanExit())
                {
                    return;
                }
                
                if (!transition.IsSatisfied(this))
                {
                    continue;
                }
                
                SwitchState(transition.TransitionTo);
                break;
            }
        }

        public void Dispose()
        {
            _updateSubscription.Dispose();

            foreach (var (_, state) in _states)
            {
                if (state is IDisposable disposable)
                {
                    disposable?.Dispose();
                }
            }
            
        }
    }
}
