namespace StateMachine
{
    public abstract partial class StateBase<TContext>
    {
        public static StateBase<TContext> Null => new NullState(default);

        private sealed class NullState : StateBase<TContext>
        {
            public NullState(TContext context) : base(context)
            {
            }
        }
    }
}