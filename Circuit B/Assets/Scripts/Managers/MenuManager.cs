using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    PlayerInput _uiInput;
    [SerializeField] TextMeshProUGUI _versionText;

    [SerializeField] List<Menus> _menus = new List<Menus>();
    [SerializeField] Animator _mainMenuAnimator, _musicMenuAnimator;
    bool _isMusicMenu;

    [SerializeField] GameObject _opening;
    [SerializeField] Button _selectedButton;

    [SerializeField] float _timeOutTime = 300;
    float _timeOut;

    public static MenuManager Instance { get; private set; }

    public UnityEvent MainMenuLoaded = new UnityEvent();

    public UnityEvent OptionsMenuLoaded = new UnityEvent();

    bool _deadMenu;
    bool _memoryMenu;

    public List<Menus> Menus { get { return _menus; } set { _menus = value; } }
    public bool MemoryMenu { get { return _memoryMenu; } }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _uiInput = new PlayerInput();
        _timeOut = _timeOutTime;
    }

    private void OnEnable()
    {
        _uiInput.UI.Enable();
        for (int i = 0; i < _uiInput.asset.actionMaps.Count; i++)
        {
            _uiInput.asset.actionMaps[i].actionTriggered += TestCheckInput;
        }
        _uiInput.UI.Pause.performed += PauseScreen;
        _uiInput.UI.Memories.performed += MemoriesScreen;
    }

    private void OnDisable()
    {
        _uiInput.UI.Disable();
        for (int i = 0; i < _uiInput.asset.actionMaps.Count; i++)
        {
            _uiInput.asset.actionMaps[i].actionTriggered -= TestCheckInput;
        }
        _uiInput.UI.Pause.performed -= PauseScreen;
        _uiInput.UI.Memories.performed -= MemoriesScreen;
    }

    private void Start()
    {
        _versionText.text = $"Version: {Application.version}";
    }

    private void Update()
    {
        if (GameStateManager.Instance.CurrentState == GameStateManager.Instance.StateFactory.MainMenu()){
            if (_timeOut > 0)
            {
                _timeOut -= Time.deltaTime;
            }
            else
            {
                if (!_isMusicMenu)
                {
                    _mainMenuAnimator.SetTrigger("toMusic");
                    _musicMenuAnimator.SetTrigger("toMusic");
                    _isMusicMenu = true;
                    AudioManager.Instance.TransitionToMenuMusic(5);
                }
            }
        }
    }

    public void TestCheckInput(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            _timeOut = _timeOutTime;
        }
    }

    public void HideMusicScreen()
    {
        _mainMenuAnimator.SetTrigger("toMenu");
        _musicMenuAnimator.SetTrigger("toMenu");
        _isMusicMenu = false;
    }

    public void NewGameButton()
    {
        if (!_isMusicMenu)
        {
            GameStateManager.Instance.CreatingNewGame = true;
        }
    }

    void PauseScreen(InputAction.CallbackContext ctx)
    {
        if (!_isMusicMenu)
        {
            if (_menus.Find(r => r.MenuName == "Memories").IsActive)
            {
                MemoriesScreen();
            }
            else if (_menus.Find(r => r.MenuName == "Main Menu").IsActive)
            {

            }
            else if (ThoughtsManager.Instance.IsShowing)
            {
                ThoughtsManager.Instance.HideThought();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                if (!GameStateManager.Instance.IsMainMenu && !_deadMenu)
                {
                    PauseScreen();
                    OptionsMenuLoaded.Invoke();
                }
            }
        }
    }

    void MemoriesScreen(InputAction.CallbackContext ctx)
    {
        MemoriesScreen();
    }

    public void MemoriesScreen()
    {
        if (!_isMusicMenu)
        {
            if (!GameStateManager.Instance.IsPaused && !_deadMenu)
            {
                if (_menus.Find(r => r.MenuName == "Memories").IsActive)
                {
                    HideAllScreenButton();
                    ShowScreenButton("In Game");
                    HideScreenButton("Memories");
                    Cursor.lockState = CursorLockMode.Locked;
                    _memoryMenu = false;
                    MemoryManager.Instance.FinalMemoryCutScene();
                }
                else
                {
                    HideAllScreenButton();
                    ShowScreenButton("In Game");
                    ShowScreenButton("Memories");
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    _memoryMenu = true;
                }
            }
        }
    }

    public void HideOpeningScreen()
    {
        //Debug.Log("Hide Called");
        _opening.GetComponent<Animator>().SetTrigger("Start");
    }

    public void DeadScreen()
    {
        _deadMenu = true;
        HideAllScreenButton();
        ShowScreenButton("In Game");
        ShowScreenButton("No Power");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DataPersistanceManager.Instance.SaveGame();
    }

    public void PauseScreen()
    {
        if (!_isMusicMenu)
        {
            if (GameStateManager.Instance.IsPaused)
            {
                HideAllScreenButton();
                ShowScreenButton("In Game");
                HideScreenButton("Pause");
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                GameStateManager.Instance.IsPaused = false;
            }
            else
            {
                HideAllScreenButton();
                ShowScreenButton("In Game");
                ShowScreenButton("Pause");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameStateManager.Instance.IsPaused = true;
            }
        }
    }

    public void BackScreenButton()
    {
        if (GameStateManager.Instance.IsMainMenu)
        {
            MainMenuScreen();
        }

        if (GameStateManager.Instance.IsPaused)
        {
            HideAllScreenButton();
            ShowScreenButton("In Game");
            ShowScreenButton("Pause");
        }
    }

    public void MainMenuScreen()
    {
        _deadMenu = false;
        HideAllScreenButton();
        ShowScreenButton("Out Of Game");
        ShowScreenButton("Main Menu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameStateManager.Instance.IsMainMenu = true;
        MainMenuLoaded.Invoke();
        OptionsMenuLoaded.Invoke();
        _selectedButton.Select();

    }

    public void ShowScreenButton(string screenName)
    {
        if (!_isMusicMenu)
        {
            _menus.Find(r => r.MenuName == screenName).IsActive = true;
        }
    }

    public void HideAllScreenButton()
    {
        if (!_isMusicMenu)
        {
            foreach (var menu in _menus)
            {
                menu.IsActive = false;
            }
        }
    }

    public void HideScreenButton(string screenName)
    {
        if (!_isMusicMenu)
        {
            _menus.Find(r => r.MenuName == screenName).IsActive = false;
        }
    }

    public void QuitGameButton()
    {
        if (!_isMusicMenu)
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }


}

[System.Serializable]
public class Menus
{
    [SerializeField] string _menuName;
    [SerializeField] int _menuNumber;
    [SerializeField] GameObject _menuObject;
    [SerializeField] MenuType _menuType;
    bool _isActive = false;

    public int MenuNumber { get { return _menuNumber; } set { _menuNumber = value; } }
    public string MenuName { get { return _menuName; } set { _menuName = value; } }
    public GameObject MenuObject { get { return _menuObject; } set { _menuObject = value; } }
    public MenuType MenuType { get { return _menuType; } set { _menuType = value; } }
    public bool IsActive
    {
        get { return _isActive; }
        set
        {
            _isActive = value;
            _menuObject.SetActive(value);
        }
    }

    public Menus(string menuName, int menuNumber, GameObject menuObject, MenuType menuType, bool isActive)
    {
        MenuName = menuName;
        MenuNumber = menuNumber;
        MenuObject = menuObject;
        MenuType = menuType;
        IsActive = isActive;
    }
}

[System.Serializable]
public enum MenuType
{
    MainMenu = 0,
    InGame = 1
}

// 204              197             167
// 0.8018868        0.7734498       0.6543699