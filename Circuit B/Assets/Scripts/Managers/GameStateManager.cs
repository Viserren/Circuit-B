using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameStateManager : MonoBehaviour
{
    bool _firstLoadComplete;
    bool _creatingNewGame;
    bool _loadingGame;
    bool _doneLoading;
    bool _inCutScene;

    bool _isPaused;
    bool _isMainMenu;

    [SerializeField] PlayableDirector _playableDirectorStartGame;
    [SerializeField] PlayableDirector _playableDirectorEndGame;

    public PlayableDirector ClipStartGame { get { return _playableDirectorStartGame; } }
    public PlayableDirector ClipEndGame { get { return _playableDirectorEndGame; } }

    public bool CreatingNewGame { get { return _creatingNewGame; } set { _creatingNewGame = value; } }
    public bool LoadingGame { get { return _loadingGame; } set { _loadingGame = value; } }
    public bool DoneLoading { get { return _doneLoading; } set { _doneLoading = value; } }
    public bool FirstLoadComplete { get { return _firstLoadComplete; } set { _firstLoadComplete = value; } }
    public bool IsPaused { get { return _isPaused; } set { _isPaused = value; } }
    public bool IsMainMenu { get { return _isMainMenu; } set { _isMainMenu = value; } }
    public bool InCutScene { get { return _inCutScene; } set { _inCutScene = value; } }
    

    public static GameStateManager Instance { get; private set; }

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


    // State machine variables
    GameBaseState _currentState;
    GameStateFactory _states;

    // Getters and setters
    public GameBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public GameStateFactory StateFactory { get {  return _states; } }

    // Start is called before the first frame update
    void Start()
    {
        // Setup the state machine
        _states = new GameStateFactory(this);
        _currentState = _states.MainMenu();
        //IsMainMenu = true;
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateStates();
        //Debug.Log($"Current State: {_currentState}");
    }

    public void LoadSceneAfter()
    {
        _currentState.EnterState();
    }

    public void DoneSceneLoad()
    {
        MenuManager.Instance.MainMenuScreen();
    }

    public void SpawnPlayer()
    {
        DoneLoading = true;
        GameObject.FindAnyObjectByType<PlayerStateManager>().SetCharacterPosition(new Vector3(33.1020012f, 0.931999981f, 51.5740013f), new Quaternion(0, -0.700010002f, 0, -0.714133084f));
        DataPersistanceManager.Instance.SaveGame();
        GameObject.FindAnyObjectByType<ClockManager>().AddSeconds(10800);
    }

    public void KillPlayer()
    {
        GameObject.FindAnyObjectByType<PlayerStateManager>().Dead();
    }
}
