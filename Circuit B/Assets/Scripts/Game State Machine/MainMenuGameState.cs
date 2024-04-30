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
        if (Context.CreatingNewGame)
        {
            SwitchState(Factory.NewGame());
        }
        else if (Context.LoadingGame)
        {
            SwitchState(Factory.LoadGame());
        }
    }
    public override void EnterState()
    {
        CameraManager.Instance.MainMenuCamera(true, this);
        Context.IsPaused = false;
        Context.CreatingNewGame = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (!Context.FirstLoadComplete)
        {
            GameSceneManager.Instance.LoadScene(1);
            MenuManager.Instance.Menus.FindAll(r => r.MenuType == MenuType.InGame).ForEach(r => { r.IsActive = false; });
            Context.FirstLoadComplete = true;
        }
    }
    public override void ExitState()
    {
        //Debug.Log("Exit Main Menu");
    }
    public override void UpdateState()
    {
        //if (MenuManager.Instance.InGameCanvas != null && MenuManager.Instance.InGameCanvas.enabled == true) 
        //{
        //    MenuManager.Instance.InGameCanvas.enabled = false;
        //}
        CheckSwitchStates();
    }
}
