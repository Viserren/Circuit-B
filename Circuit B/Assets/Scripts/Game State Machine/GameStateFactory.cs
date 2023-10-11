using System.Collections.Generic;

public class GameStateFactory
{
    GameStateManager _context;
    Dictionary<string, GameBaseState> _states = new Dictionary<string, GameBaseState>();

    public GameStateFactory(GameStateManager currentContext)
    {
        _context = currentContext;
        _states.Add("Playing", new GamePlayngState(_context, this));
        _states.Add("Paused", new GamePausedState(_context, this));
        _states.Add("NotPlaying", new GameNotPlayingState(_context, this));
    }

    public GameBaseState Playing()
    {
        return _states["Playing"];
    }
    public GameBaseState Paused()
    {
        return _states["Paused"];
    }
    public GameBaseState NotPlaying()
    {
        return _states["NotPlaying"];
    }
}
