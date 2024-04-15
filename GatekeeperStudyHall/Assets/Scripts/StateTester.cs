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
    List<PlayerSO> players;

    public TestEnterState(List<PlayerSO> players) {
        this.players = players;
    }

    public void Enter() => Debug.Log($"Hello from TestEnterState, there are {players.Count} players");
}


public class TestUpdateState : IState
{
    public void Update() => Debug.Log("Hello from TestUpdateState");
}


public class TestExitState : IState
{
    public void Exit() => Debug.Log("Hello from TestExitState");
}


public class TimerState : IState
{
    float timer = 0f;

    public void Enter() => timer = 0f;

    public void Update() => timer += Time.deltaTime;

    public void Exit() => Debug.Log($"Time in this state: {timer:N2}s");
}


public class StateTester : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    List<PlayerSO> players;

    TestEnterState enter;
    TestExitState exit;
    TestUpdateState update;
    TimerState timer;

    IState currentState;

    void Start() 
    {
        players = playerListObject.list;

        enter = new(players);
        exit = new();
        update = new();
        timer = new();

        currentState = enter;
        currentState.Enter();
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
        if (Input.GetKeyUp(KeyCode.Alpha4)) {
            nextState = timer;
        }

        return nextState;
    }
}
