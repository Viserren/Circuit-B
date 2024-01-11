using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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
        Context.DoneLoading = true;
    }
}
