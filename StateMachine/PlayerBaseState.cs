public abstract class PlayerBaseState 
{
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSubState;
    private PlayerBaseState _currentSuperState;
    private bool _isRootState = false;
    protected PlayerStateMachine Ctx {get {return _ctx;}}
    public PlayerBaseState CurrentSubState{get {return _currentSubState;}}
    protected PlayerStateFactory Factory {get {return _factory;}}
    protected bool IsRootState {set {_isRootState = value;}}
    public PlayerBaseState(PlayerStateMachine currentContext,
     PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();
    public abstract void FixedUpdateState();

    public void FixedUpdateStates()
    {
        FixedUpdateState();
        if(_currentSubState != null)
        {
            _currentSubState.FixedUpdateState();
        }
    }
    public void UpdateStates(){
        UpdateState();
        if(_currentSubState != null)
        {
            _currentSubState.UpdateState();
        }
    }
    protected void SwitchState(PlayerBaseState newState)
    {
        ExitState();
        
        newState.EnterState();
        if(_isRootState)
        {
            //switch current state of context
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
        
    }
    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}   
