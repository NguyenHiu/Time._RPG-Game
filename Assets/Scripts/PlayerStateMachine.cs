using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine 
{
    public PlayerState currentState {  get; private set; }

    public void Initialize(PlayerState state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void ChangeState(PlayerState state)
    {
        currentState.Exit(); 
        currentState = state;
        currentState.Enter();
    }
}
