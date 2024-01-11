using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameStateManager : MonoBehaviour
{
    bool _loadedWorld;
    bool _creatingNewGame;
    bool _loadingGame;
    bool _doneLoading;

    bool _isPaused;
    bool _isMainMenu;

    public bool CreatingNewGame { get { return _creatingNewGame; } set { _creatingNewGame = value; } }
    public bool LoadingGame { get { return _loadingGame; } set { _loadingGame = value; } }
    public bool DoneLoading { get { return _doneLoading; } set { _doneLoading = value; } }
    public bool LoadedWorld { get { return _loadedWorld; } set { _loadedWorld = value; } }
    public bool IsPaused { get { return _isPaused; } set { _isPaused = value; } }
    public bool IsMainMenu { get { return _isMainMenu; } set { _isMainMenu = value; } }

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
        IsMainMenu = true;
        _currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateStates();
        //Debug.Log($"Current State: {_currentState}");
    }
}
