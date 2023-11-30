using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{
    //public UnityEvent OnMainMenu;
    [SerializeField] Canvas _mainMenuCanvas;
    [SerializeField] TextMeshProUGUI _versionText;

    public static MenuManager Instance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _versionText.text = $"Version: {Application.version}";
    }

    public void LoadGameButton()
    {
        // TODO: Add code to load the last saved game
    }

    public void NewGameButton()
    {
        // TODO: Add code to start a new game

        GameStateManager.Instance.CurrentState = GameStateManager.Instance.StateFactory.NewGame();
        GameStateManager.Instance.CurrentState.EnterState();
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
        //OnMainMenu.Invoke();
        // TODO: Add code to show the main menu
    }
}
