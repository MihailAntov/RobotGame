using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
   public PlayerIdleState(PlayerStateMachine currentContext,
                         PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory){}
   public override void EnterState()
    {
        Ctx.Animator.SetBool("isWalking",false);
        Ctx.Animator.SetBool("isRunning",false);
        //Debug.Log("Enter idle");
        
            Ctx.AppliedMovementX = 0;
            Ctx.AppliedMovementZ = 0;
        Ctx.footstep.volume = 0;
      
        
    }
    public override void UpdateState()
    {
        if(Ctx.IsPlaying("Rolling"))
        {
            Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * Ctx.RunSpeed;
            Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.RunSpeed;
        
        Ctx.AppliedMovement = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f) * Ctx.AppliedMovement;
        }
        else
        {
            Ctx.AppliedMovementX = 0;
            Ctx.AppliedMovementZ = 0;
        }
        
        
        //lock jump after roll
        if(Ctx.JumpLockTimer > 5)
        {
            Ctx.IsJumpLocked = false;
            Ctx.JumpLockTimer = 0;
            //Debug.Log("Jump unlocked");
        }
        //CheckSwitchStates();
    }
    public override void ExitState()
    {
        //Debug.Log("Exit idle");
    }
    public override void FixedUpdateState()
    {
        CheckSwitchStates();
        if(Ctx.IsJumpLocked)
        {
            Ctx.JumpLockTimer++;
        }
        Ctx.DetectSurface();
    }
    public override void InitializeSubState()
    {
        
    }
    public override void CheckSwitchStates()
    {
        //if(Ctx.IsMovementPressed && Ctx.IsRunPressed && !Ctx.IsHanging && !Ctx.IsClimbing)
        if (Ctx.IsRolling)
        {
            SwitchState(Factory.Roll());
        }
        else if (Ctx.IsLanding)
        {
            SwitchState(Factory.Land());
        }
        else if (Ctx.IsMovementPressed && Ctx.IsRunPressed )
        {
            SwitchState(Factory.Run());
        }
        //else if (Ctx.IsMovementPressed && !Ctx.IsHanging && !Ctx.IsClimbing)
        else if (Ctx.IsMovementPressed )
        {
            SwitchState(Factory.Walk());
        }
    }
}
