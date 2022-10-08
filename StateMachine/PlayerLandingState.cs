using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandingState : PlayerBaseState
{
    int landCounter;
    // Start is called before the first frame update
    public PlayerLandingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory){}
    public override void EnterState()
    {
        InitializeSubState();
        landCounter = 0;
        //Debug.Log("Landing enter");
        Ctx.Animator.SetBool("isLanding", true);
        Ctx.AudioManager.Play("land");
        
    }
    public override void ExitState()
    {
        Ctx.Animator.SetBool("isLanding", false);
        Ctx.IsLanding = false;
        landCounter = 0;
        //Debug.Log("Landing Exit");
    }
    public override void FixedUpdateState()
    {
        landCounter++;
        Ctx.DetectSurface();
    }
    public override void UpdateState()
    {
       CheckSwitchStates();
       //landCounter++;
    }
    public override void CheckSwitchStates()
    {
        if (landCounter > 40)
        {
            if (Ctx.IsMovementPressed && Ctx.IsRunPressed)
            {
                SwitchState(Factory.Run());
            }
            else if (Ctx.IsMovementPressed)
            {
                SwitchState(Factory.Walk());
            }
            else
            {
                SwitchState(Factory.Idle());
            }
        }
    }
    public override void InitializeSubState()
    {
        
    }

}
