using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ONLY IN THIS STATE IF IN MAIN MENU
public class MainMenuGameState : GameBaseState
{
    public MainMenuGameState(GameStateManager currentContext, GameStateFactory gameStateFactory) : base(currentContext, gameStateFactory)
    {
    }
    public override void CheckSwitchStates()
    {
        if (Context.CurrentState == Context.StateFactory.NewGame())
        {
            SwitchState(Factory.NewGame());
        }
        else if(Context.CurrentState == Context.StateFactory.LoadGame())
        {
            SwitchState(Factory.LoadGame());
        }
    }
    public override void EnterState()
    {
        GameSceneManager.Instance.LoadScene(1, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
    public override void ExitState()
    {
        
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
