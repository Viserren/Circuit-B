using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ONLY IN THIS STATE IF IN MAIN MENU
public class NewGameState : GameBaseState
{
    public NewGameState(GameStateManager currentContext, GameStateFactory gameStateFactory) : base(currentContext, gameStateFactory)
    {
    }
    public override void CheckSwitchStates()
    {
        if (Context.CurrentState == Context.StateFactory.Playing())
        {
            SwitchState(Factory.Playing());
        }
    }
    public override void EnterState()
    {
        // TODO: create new game file
        Debug.Log("Creating Game");
    }
    public override void ExitState()
    {
        // TODO: switch to playing once file created
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
