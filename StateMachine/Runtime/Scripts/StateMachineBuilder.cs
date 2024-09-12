using System;
using System.Collections.Generic;
using StateMachine.Abstractions;

namespace StateMachine
{
    
/* new Builder<TContext>()
 *      .State(state, ctx => {
 *                  ctx.AddTransition(newInnerState, condition)
 *              })
 *          .TransitionTo(newState, condition)
 *          .Build(context)
 */

    public interface IStateMachineBuilder<TContext>
    {
        StateMachine<TContext> Build(TContext context);

        ITransitionBuilder<TState, TContext> State<TState>(TState state)
            where TState : IState<TContext>;
    }

    public interface ITransitionBuilder<TState, TContext> : IStateMachineBuilder<TContext>
        where TState : IState<TContext>
    {
        ITransitionBuilder<TState, TContext> TransitionTo(IState<TContext> newState, Func<bool> condition);
    }


    public class StateMachineBuilder<TContext> : IStateMachineBuilder<TContext>
    {
        public List<Transition<TContext>> Transitions { get; } = new();
    
        public StateMachine<TContext> Build(TContext context)
        {
            throw new NotImplementedException();
        }

        public ITransitionBuilder<TState, TContext> State<TState>(TState state) where TState : IState<TContext>
        {
            throw new System.NotImplementedException();
        }
    }

    public class StateMachineTransitionBuilder<TState, TContext> : StateMachineBuilder<TContext>, ITransitionBuilder<TState, TContext>
        where TState : IState<TContext>
    {
        private readonly TState _state;

        public StateMachineTransitionBuilder(TState state)
        {
            _state = state;
        }

        public ITransitionBuilder<TState, TContext> TransitionTo(IState<TContext> newState, Func<bool> condition)
        {
            Transitions.Add(new Transition<TContext>(_state, newState, condition));
            return this;
        }
    }
}