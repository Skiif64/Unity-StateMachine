using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using StateMachine;
using StateMachine.Runtime.Tests.Fixtures;
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
        _stateMachine.Update();
        Assert.That(_stateMachine.CurrentState, Is.TypeOf<MockState2>());
        
    }
    
    [Test]
    public void ConditionUnchanged_ShouldNotTransit()
    {
        var state1 = new MockState1(_context);
        var state2 = new MockState2(_context);
        
        _stateMachine.FromState(state1, new Transition<MockContext>(state2, () => _context.Flag1));
        _stateMachine.Init(state1);

        _stateMachine.Update();
        Assert.That(_stateMachine.CurrentState, Is.TypeOf<MockState1>());
        
    }
}
