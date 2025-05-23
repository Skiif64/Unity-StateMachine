using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using StateMachine;
using StateMachine.Abstractions;
using StateMachine.Runtime.Tests.Fixtures;
using StateMachine.Transitions;
using UnityEngine;
using UnityEngine.TestTools;

public class StateMachineTests
{
    private MockContext _context;
    private StateMachine<MockContext> _stateMachine;
    
    [SetUp]
    public void Setup()
    {
        _context = new MockContext();
        _stateMachine = new StateMachine<MockContext>();
    }
    
    [Test]
    public void Init_ShouldBeInInitialState()
    {
        var initialState = new MockState1(_context);
        _stateMachine.Init(initialState);
        
        Assert.That(_stateMachine.CurrentState, Is.EqualTo(initialState));
    }
    
    
    [Test]
    public void ConditionChanged_ShouldTransit()
    {
        var state1 = new MockState1(_context);
        var state2 = new MockState2(_context);
        
        _stateMachine.FromState(state1, new Transition<MockContext>(state2, () => _context.Flag1));
        
        _stateMachine.Init(state1);

        _context.Flag1 = true;
        _stateMachine.CheckTransitions();
        Assert.That(_stateMachine.CurrentState, Is.TypeOf<MockState2>());
        
    }
    
    [Test]
    public void ConditionUnchanged_ShouldNotTransit()
    {
        var state1 = new MockState1(_context);
        var state2 = new MockState2(_context);
        
        _stateMachine.FromState(state1, new Transition<MockContext>(state2, () => _context.Flag1));
        _stateMachine.Init(state1);

        _stateMachine.CheckTransitions();
        Assert.That(_stateMachine.CurrentState, Is.TypeOf<MockState1>());
        
    }

    [Test]
    public void InnerState_TransitionShouldCheck()
    {
        var state1 = new MockState1(_context);
        var state2 = new MockHierarchicalState(_context, state1);
        
        _stateMachine.AnyState(new Transition<MockContext>(state2, () => true));
        
        _stateMachine.Init(state2);

        _context.Flag2 = true;
        
        _stateMachine.CheckTransitions();
        
        Assert.That(((IStateMachine<MockContext>)_stateMachine.CurrentState).CurrentState, Is.TypeOf<MockState2>());
    }
}
