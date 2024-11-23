using StateMachine.Abstractions;

namespace StateMachine.Transitions
{
    public struct ObserverTransition<TContext> : ITransition<TContext>
    {
        private readonly bool _resetOnSatisfy;
        private bool _signaling;
        
        public IState<TContext> TransitionTo { get; }

        public ObserverTransition(IState<TContext> transitionTo, bool resetOnSatisfy = true)
        {
            TransitionTo = transitionTo;
            _resetOnSatisfy = resetOnSatisfy;
            _signaling = false;
        }
        
        public bool IsSatisfied(IStateMachine<TContext> stateMachine)
        {
            if (_signaling)
            {
                if (_resetOnSatisfy)
                {
                    Reset();
                }
                
                return true;
            }

            return false;
        }

        public void Signal() => _signaling = true;

        public void Reset() => _signaling = false;
    }
}