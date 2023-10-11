using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ManuManager : MonoBehaviour
{
    public UnityEvent OnMainMenu;

    public void LoadGameButton()
    {
        // TODO: Add code to load the last saved game
    }

    public void NewGameButton()
    {
        // TODO: Add code to start a new game
    }

    public void SettingsButton()
    {
        // TODO: Add code to show the settings menu
    }

    public void QuitGameButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void MainMenu()
    {
        OnMainMenu.Invoke();
        // TODO: Add code to show the main menu
    }
}
