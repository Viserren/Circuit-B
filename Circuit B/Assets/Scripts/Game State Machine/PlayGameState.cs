using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameState : GameBaseState
{
    public PlayGameState(GameStateManager currentContext, GameStateFactory gameStateFactory) : base(currentContext, gameStateFactory)
    {

    }

    public override void CheckSwitchStates()
    {
        if (Context.CurrentState == Context.StateFactory.Paused())
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
