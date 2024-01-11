using System.Collections.Generic;

public class PlayerStateFactory
{
    PlayerStateManager _context;
    Dictionary<string, PlayerBaseState> _states = new Dictionary<string, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateManager currentContext)
    {
        _context = currentContext;
        _states.Add("Idle", new PlayerIdleState(_context, this));
        _states.Add("Walk", new PlayerWalkState(_context, this));
        _states.Add("Run", new PlayerRunState(_context, this));
        _states.Add("Jump", new PlayerJumpState(_context, this));
        _states.Add("Grounded", new PlayerGroundedState(_context, this));
        _states.Add("Falling", new PlayerFallState(_context, this));
        _states.Add("Dead", new PlayerDeadState(_context, this));
    }
    public PlayerBaseState Dead()
    {
        return _states["Dead"];
    }
    public PlayerBaseState Idle()
    {
        return _states["Idle"];
    }
    public PlayerBaseState Walk()
    {
        return _states["Walk"];
    }
    public PlayerBaseState Run()
    {
        return _states["Run"];
    }
    public PlayerBaseState Jump()
    {
        return _states["Jump"];
    }
    public PlayerBaseState Grounded()
    {
        return _states["Grounded"];
    }
    public PlayerBaseState Falling()
    {
        return _states["Falling"];
    }

}
