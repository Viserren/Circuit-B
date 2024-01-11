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
        if (Context.IsPaused)
        {
            SwitchState(Factory.Paused());
        }
    }

    public override void EnterState()
    {
        Context.DoneLoading = false;
        Context.IsMainMenu = false;
        Debug.Log("Enter Playing");
        CameraManager.Instance.MainMenuCamera(false, this);
        MenuManager.Instance.Menus.FindAll(r => r.MenuType == MenuType.MainMenu).ForEach(r => { r.IsActive = false; }); 
        MenuManager.Instance.Menus.FindAll(r => r.MenuType == MenuType.InGame).ForEach(r => { r.IsActive = false; });
        MenuManager.Instance.Menus.FindAll(r => r.MenuType == MenuType.InGame).Find(r => r.MenuName == "In Game").IsActive = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void ExitState()
    {
        Debug.Log("Exit Playing");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
