namespace StateMachine.Abstractions
{
    public interface IState<TContext>
    {
        bool CanExit { get; }
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }

    public interface IState<TParent, TContext> : IState<TContext>
        where TParent : IState<TContext>
    {
        TParent Parent { get; }
    }
}