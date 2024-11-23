
namespace StateMachine.Abstractions
{
    public interface IStateMachine<TContext> : ITransitionable<TContext>
    {
        IState<TContext> CurrentState { get; }
        void Init(IState<TContext> initialState);
        void Update();
        void SwitchState(IState<TContext> newState);
    }
}