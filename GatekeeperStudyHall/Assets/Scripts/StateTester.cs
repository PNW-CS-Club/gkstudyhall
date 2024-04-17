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


public class TraitRollState : IState
{
    List<PlayerSO> players;
    int roll = 4; // I didnt know how we are tracking what the player gets when they roll.

    public TraitRollState(List<PlayerSO> players) {
        this.players = players;
    }

    
    public void Enter() {
        roll = 4; 
        TraitHandler.ActivateTrait(players[0], roll);
    }

    public void Exit() => Debug.Log("Testing TraitRollState");

}


public class StateMachine
{
    List<PlayerSO> players;

    public TestEnterState enterState;
    public TestExitState exitState;
    public TestUpdateState updateState;
    public TimerState timerState;
    public TraitRollState traitRollState;

    IState currentState;

    public StateMachine(List<PlayerSO> players) 
    {
        this.players = players;

        enterState = new(players);
        exitState = new();
        updateState = new();
        timerState = new();
        traitRollState = new(players);
    }

    public void Initialize(IState state) 
    {
        currentState = state;
        currentState.Enter();
    }

    public void TransitionTo(IState state) 
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }

    public void Update() 
    {
        currentState.Update();
    }
}


class StateTester : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    StateMachine stateMachine;

    void Start() 
    {
        stateMachine = new StateMachine(playerListObject.list);
        stateMachine.Initialize(stateMachine.enterState);
    }

    void Update() 
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) {
            stateMachine.TransitionTo(stateMachine.enterState);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2)) {
            stateMachine.TransitionTo(stateMachine.updateState);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3)) {
            stateMachine.TransitionTo(stateMachine.exitState);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4)) {
            stateMachine.TransitionTo(stateMachine.timerState);
        }
        if (Input.GetKeyUp(KeyCode.Alpha5)) {
            stateMachine.TransitionTo(stateMachine.traitRollState);
        }

        stateMachine.Update();
    }
}
