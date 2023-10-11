public abstract class GameBaseState
{
    //private bool _isRootState = false;
    private GameStateManager _context;
    private GameStateFactory _factory;

    //protected bool IsRootState { set { _isRootState = value; } }
    protected GameStateManager Context { get { return _context; } }
    protected GameStateFactory Factory { get { return _factory; } }

    public GameBaseState(GameStateManager currentContext, GameStateFactory gameStateFactory)
    {
        _context = currentContext;
        _factory = gameStateFactory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();

    public void UpdateStates()
    {
        UpdateState();
    }

    protected void SwitchState(GameBaseState newState)
    {
        // Current state exits
        ExitState();

        // New state enters
        newState.EnterState();
    }
}
