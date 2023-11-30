using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedGameState : GameBaseState
{
    public PausedGameState(GameStateManager currentContext, GameStateFactory gameStateFactory) : base(currentContext, gameStateFactory)
    {

    }

    public override void CheckSwitchStates()
    {
        if (Context.CurrentState == Context.StateFactory.Playing())
        {
            SwitchState(Factory.Playing());
        }
        else if (Context.CurrentState == Context.StateFactory.MainMenu())
        {
            SwitchState(Factory.MainMenu());
        }
    }

    public override void EnterState()
    {
        Time.timeScale = 0;
    }

    public override void ExitState()
    {
        Time.timeScale = 1;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
