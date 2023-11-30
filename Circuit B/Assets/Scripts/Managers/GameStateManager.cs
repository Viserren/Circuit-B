using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameStateManager : MonoBehaviour
{
    //public GameState state { get; private set; }

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
        _currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateStates();
    }

    //public void SwitchState(int newState)
    //{
    //    try
    //    {
    //        state = (GameState)newState;
    //        Debug.Log("state is = " + state.ToString());
    //        _currentState = _states.
    //        _currentState.EnterState();
    //    }
    //    catch
    //    {
    //        Debug.LogError("Invalid state");
    //        return;
    //    }
    //}
}


//public enum GameState
//{
//    MainMenu = 0,
//    Playing = 1,
//    Paused = 2,
//    NewGame = 3,
//    LoadGame = 4
//}
