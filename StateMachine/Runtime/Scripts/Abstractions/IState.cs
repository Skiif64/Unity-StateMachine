namespace StateMachine.Abstractions
{
    public interface IState<TContext>
    {
        bool CanExit { get; }
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}