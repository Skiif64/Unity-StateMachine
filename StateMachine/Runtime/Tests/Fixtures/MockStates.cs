using System.Collections.Generic;

namespace StateMachine.Runtime.Tests.Fixtures
{
    internal class MockState1 : StateBase<MockContext>
    {
        public MockState1(MockContext context) : base(context)
        {
        }
    }
    
    internal class MockState2 : StateBase<MockContext>
    {
        public MockState2(MockContext context) : base(context)
        {
        }
    }

    internal class MockHierarchicalState1 : HierarchicalStateBase<MockContext>
    {
        public MockHierarchicalState1(MockContext context) : base(context, new MockState1(context))
        {
        }
    }
    
    internal class MockHierarchicalState2 : HierarchicalStateBase<MockContext>
    {
        public MockHierarchicalState2(MockContext context) : base(context, new MockState1(context))
        {
        }
    }
}