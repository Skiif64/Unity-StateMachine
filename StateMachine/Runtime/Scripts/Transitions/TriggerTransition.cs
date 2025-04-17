using StateMachine.Abstractions;

namespace StateMachine.Transitions
{
    public class TriggerTransition<TContext> : ITransition<TContext>
    {
        private bool _triggered;
        
        public IState<TContext> TransitionTo { get; }

        public TriggerTransition(IState<TContext> transitionTo)
        {
            TransitionTo = transitionTo;
            _triggered = false;
        }
        
        public bool IsSatisfied(IStateMachine<TContext> stateMachine)
        {
            if (!_triggered)
            {
                return false;
            }

            _triggered = false;
            return true;
        }

        public void Trigger() => _triggered = true;
    }
}