using StateMachine.Abstractions;
using StateMachine.Transitions;

namespace StateMachine.Runtime.Tests.Fixtures
{
    internal class MockHierarchicalState : HierarchicalStateBase<MockContext>
    {
        public MockHierarchicalState(MockContext context, IState<MockContext> initialState) : base(context, initialState)
        {
            SetupStateMachine(context);
        }

        private void SetupStateMachine(MockContext context)
        {
            var state2 = new MockState2(context);
            AnyState(Transition.Create(state2, () => context.Flag2));
        }
    }
}