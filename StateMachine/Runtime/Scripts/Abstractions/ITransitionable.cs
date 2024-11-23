namespace StateMachine.Abstractions
{
    public interface ITransitionable<TContext>
    {
        void FromState(IState<TContext> from, ITransition<TContext> transition);
        void AnyState(ITransition<TContext> transition);
    }
}