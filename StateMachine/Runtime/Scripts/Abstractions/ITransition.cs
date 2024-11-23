namespace StateMachine.Abstractions
{
    public interface ITransition<TContext>
    {
        IState<TContext> TransitionTo { get; }
        bool IsSatisfied(IStateMachine<TContext> stateMachine);
    }
}