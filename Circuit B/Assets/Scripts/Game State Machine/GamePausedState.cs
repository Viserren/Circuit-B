using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePausedState : GameBaseState
{
    public GamePausedState(GameStateManager currentContext, GameStateFactory gameStateFactory) : base(currentContext, gameStateFactory)
    {

    }

    public override void CheckSwitchStates()
    {
        if (Context.state == GameState.Playing)
        {
            SwitchState(Factory.Playing());
        }
        else if (Context.state == GameState.NotPlaying)
        {
            SwitchState(Factory.NotPlaying());
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
