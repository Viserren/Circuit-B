using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ONLY IN THIS STATE IF IN MAIN MENU
public class LoadGameState : GameBaseState
{
    public LoadGameState(GameStateManager currentContext, GameStateFactory gameStateFactory) : base(currentContext, gameStateFactory)
    {
    }
    public override void CheckSwitchStates()
    {
        if (Context.DoneLoading)
        {
            SwitchState(Factory.Playing());
        }
    }
    public override void EnterState()
    {
        // TODO: load game save file
        //Debug.Log("Loading Game");
        Context.LoadingGame = false;
    }
    public override void ExitState()
    {
        // TODO: once loaded, enter game with values

    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
