using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayngState : GameBaseState
{
    public GamePlayngState(GameStateManager currentContext, GameStateFactory gameStateFactory) : base(currentContext, gameStateFactory)
    {

    }

    public override void CheckSwitchStates()
    {
        if (Context.state == GameState.Paused)
        {
            SwitchState(Factory.Paused());
        }
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
