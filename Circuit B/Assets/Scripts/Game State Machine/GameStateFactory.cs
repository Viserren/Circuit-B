using System.Collections.Generic;

public class GameStateFactory
{
    GameStateManager _context;
    Dictionary<string, GameBaseState> _states = new Dictionary<string, GameBaseState>();

    public Dictionary<string, GameBaseState> States { get; }

    public GameStateFactory(GameStateManager currentContext)
    {
        _context = currentContext;
        _states.Add("Playing", new PlayGameState(_context, this));
        _states.Add("Paused", new PausedGameState(_context, this));
        _states.Add("MainMenu", new MainMenuGameState(_context, this));
        _states.Add("NewGame", new NewGameState(_context, this));
        _states.Add("LoadGame", new LoadGameState(_context, this));
    }

    public GameBaseState Playing()
    {
        return _states["Playing"];
    }
    public GameBaseState Paused()
    {
        return _states["Paused"];
    }
    public GameBaseState MainMenu()
    {
        return _states["MainMenu"];
    }
    public GameBaseState NewGame()
    {
        return _states["NewGame"];
    }
    public GameBaseState LoadGame()
    {
        return _states["LoadGame"];
    }
}
