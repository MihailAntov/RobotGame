using System.Collections.Generic;
public class PlayerStateFactory 
{
    PlayerStateMachine _context;
    enum PlayerStates {
        idle,
        walk,
        run,
        grounded,
        jump,
        fall, 
        crouch,
        hang, 
        roll,
        land,
        drop,
        climb
    }
    Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();
    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        _states[PlayerStates.idle] = new PlayerIdleState(_context, this);
        _states[PlayerStates.walk] = new PlayerWalkingState(_context, this);
        _states[PlayerStates.run] = new PlayerRunningState(_context, this);
        _states[PlayerStates.jump] = new PlayerJumpingState(_context, this);
        _states[PlayerStates.grounded] = new PlayerGroundedState(_context, this);
        _states[PlayerStates.fall] = new PlayerFallingState(_context, this);
        _states[PlayerStates.crouch] = new PlayerCrouchState(_context, this);
        _states[PlayerStates.hang] = new PlayerHangingState(_context, this);
        _states[PlayerStates.roll] = new PlayerRollingState(_context, this);
        _states[PlayerStates.climb] = new PlayerClimbingState(_context, this);
        _states[PlayerStates.land] = new PlayerLandingState(_context, this);
        _states[PlayerStates.drop] = new PlayerDroppingState(_context, this);
    }
    public PlayerBaseState Idle(){
        return _states[PlayerStates.idle] ;
    }
    public PlayerBaseState Walk(){
        return  _states[PlayerStates.walk];
    }
    public PlayerBaseState Run(){
        return  _states[PlayerStates.run];
    }
    public PlayerBaseState Jump(){
        return _states[PlayerStates.jump];
    }
    public PlayerBaseState Grounded(){
        return _states[PlayerStates.grounded];
    }
    public PlayerBaseState Fall(){
        return _states[PlayerStates.fall];
    }
    public PlayerBaseState Crouch(){
        return _states[PlayerStates.crouch];
    }

    public PlayerBaseState Hang(){
        return _states[PlayerStates.hang];
    }
    public PlayerBaseState Roll(){
        return _states[PlayerStates.roll];
    }
    public PlayerBaseState Climb(){
        return _states[PlayerStates.climb];
    }
    public PlayerBaseState Land(){
        return _states[PlayerStates.land];
    }

    public PlayerBaseState Drop(){
        return _states[PlayerStates.drop];
    }
}
