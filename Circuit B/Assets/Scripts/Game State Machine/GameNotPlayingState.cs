using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ONLY IN THIS STATE IF IN MAIN MENU
public class GameNotPlayingState : GameBaseState
{
    public GameNotPlayingState(GameStateManager currentContext, GameStateFactory gameStateFactory) : base(currentContext, gameStateFactory)
    {
    }
    public override void CheckSwitchStates()
    {
        if (Context.state == GameState.Playing)
        {
            SwitchState(Factory.Playing());
        }
    }
    public override void EnterState()
    {
        
    }
    public override void ExitState()
    {
        
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
