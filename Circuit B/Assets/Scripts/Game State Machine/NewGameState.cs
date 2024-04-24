using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

// ONLY IN THIS STATE IF IN MAIN MENU
public class NewGameState : GameBaseState
{
    public NewGameState(GameStateManager currentContext, GameStateFactory gameStateFactory) : base(currentContext, gameStateFactory)
    {

    }
    public override void CheckSwitchStates()
    {
        if (Context.DoneLoading)
        {
            SwitchState(Factory.Playing());
        }
    }
    public override async void EnterState()
    {
        // TODO: create new game file
        //Debug.Log("Creating Game");
        Context.CreatingNewGame = false;
        // Wait for game to be created

        await CreateFile();

    }
    public override void ExitState()
    {
        // TODO: switch to playing once file created
        //Debug.Log("Exit New Game");
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    async Task CreateFile()
    {
        DataPersistanceManager.Instance.NewGame();
        await Task.Delay(1000);

        MenuManager.Instance.Menus.FindAll(r => r.MenuType == MenuType.MainMenu).ForEach(r => { r.IsActive = false; });
        MenuManager.Instance.Menus.FindAll(r => r.MenuType == MenuType.InGame).ForEach(r => { r.IsActive = false; });
        MenuManager.Instance.Menus.FindAll(r => r.MenuType == MenuType.InGame).Find(r => r.MenuName == "Console Panel").IsActive = true;
        MenuManager.Instance.Menus.FindAll(r => r.MenuType == MenuType.InGame).Find(r => r.MenuName == "Thoughts Panel").IsActive = true;
        Context.Clip.Play();
        Context.Clip.stopped += OnPlayableDirectorStopped;
    }

    public void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (Context.Clip == director)
        {
            Context.DoneLoading = true;
            GameObject.FindAnyObjectByType<PlayerStateManager>().SetCharacterPosition(new Vector3(33.1020012f, 0.931999981f, 51.5740013f), new Quaternion(0, -0.700010002f, 0, -0.714133084f));
            MenuManager.Instance.Menus.FindAll(r => r.MenuType == MenuType.InGame).Find(r => r.MenuName == "Console Panel").IsActive = false;
            MenuManager.Instance.Menus.FindAll(r => r.MenuType == MenuType.InGame).Find(r => r.MenuName == "Thoughts Panel").IsActive = false;
        }
    }
}
