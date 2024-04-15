using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IState
{
    // Called when the state machine changes to this state.
    public void Enter() { }

    // Called each frame that this state is the current state.
    public void Update() { }

    // Called when the state machine changes from this state to another one.
    public void Exit() { }
}


public class TestEnterState : IState
{
    public void Enter() 
    {
        Debug.Log("Hello from TestEnterState");
    }
}


public class TestUpdateState : IState
{
    public void Update() 
    {
        Debug.Log("Hello from TestUpdateState");
    }
}


public class TestExitState : IState
{
    public void Exit() 
    {
        Debug.Log("Hello from TestExitState");
    }
}


public class StateTester : MonoBehaviour
{
    TestEnterState enter = new();
    TestExitState exit = new();
    TestUpdateState update = new();

    IState currentState;

    void Start() 
    {
        currentState = enter;
    }

    void Update() 
    {
        IState nextState = DoLogic();

        if (nextState != null) 
        {
            currentState.Exit();
            currentState = nextState;
            currentState.Enter();
        }

        // note that the state changes BEFORE its update method is called
        currentState.Update();
    }

    private IState DoLogic() 
    {
        // null value means maintain current state
        IState nextState = null;

        if (Input.GetKeyUp(KeyCode.Alpha1)) {
            nextState = enter;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2)) {
            nextState = update;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3)) {
            nextState = exit;
        }

        return nextState;
    }
}
