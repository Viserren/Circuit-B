using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManuManager : MonoBehaviour
{
    UIDocument _mainMenuDocument;
    Button _loadGameButton;
    Button _newGameButton;
    Button _settingsButton;
    Button _quitGameButton;

    private void OnEnable()
    {
        _mainMenuDocument = GetComponent<UIDocument>();

        _loadGameButton = _mainMenuDocument.rootVisualElement.Q("loadGameButton") as Button;
        _newGameButton = _mainMenuDocument.rootVisualElement.Q("newGameButton") as Button;
        _settingsButton = _mainMenuDocument.rootVisualElement.Q("settingsButton") as Button;
        _quitGameButton = _mainMenuDocument.rootVisualElement.Q("quitButton") as Button;

        _loadGameButton.RegisterCallback<ClickEvent>(LoadGameButton);
        _newGameButton.RegisterCallback<ClickEvent>(NewGameButton);
        _settingsButton.RegisterCallback<ClickEvent>(SettingsButton);
        _quitGameButton.RegisterCallback<ClickEvent>(QuitGameButton);
    }

    private void OnDisable()
    {
        _loadGameButton.UnregisterCallback<ClickEvent>(LoadGameButton);
        _newGameButton.UnregisterCallback<ClickEvent>(NewGameButton);
        _settingsButton.UnregisterCallback<ClickEvent>(SettingsButton);
        _quitGameButton.UnregisterCallback<ClickEvent>(QuitGameButton);
    }
    void LoadGameButton(ClickEvent clickEvent)
    {

    }

    void NewGameButton(ClickEvent clickEvent)
    {

    }

    void SettingsButton(ClickEvent clickEvent)
    {

    }

    void QuitGameButton(ClickEvent clickEvent)
    {
        Debug.Log("Quit");
    }
}
