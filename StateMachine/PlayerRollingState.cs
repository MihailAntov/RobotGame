using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollingState : PlayerBaseState
{
    public int rollCounter;
    public PlayerRollingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        
        Ctx.AnimationTimer = 0;
        Debug.Log("Rolling Enter");
        Ctx.IsJumpLocked = true;
        Ctx.JumpLockTimer = 0;
        //Debug.Log("Jump Locked");
        Ctx.footstep.volume = 0;
        Ctx.CharacterController.height = 1f;
        Ctx.CharacterController.radius = 1f;
        Ctx.AudioManager.Play("roll");
        if(!Ctx.IsRunPressed)
          {
            Ctx.AppliedMovement *= Ctx.RunSpeed / Ctx.WalkSpeed;
          }
        
               
    }

    public override void ExitState()
    {
        Ctx.Animator.SetBool("isRolling", false);
        Ctx.AnimationTimer = 0;
        Ctx.IsRolling = false;
        Ctx.CharacterController.height = 2f;
        Ctx.CharacterController.radius = 0.2f;
        Debug.Log("Rolling Exit");

    }
    public override void FixedUpdateState()
    {
       Ctx.AnimationTimer++;
       Ctx.DetectSurface();
        
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        // ----------------
        //Debug.Log(Ctx.AnimationTimer);
        if(!Ctx.CharacterController.isGrounded)
        {
            Ctx.AnimationTimer = 0;
        }
        
        
    }
    public override void CheckSwitchStates()
    {
         if (Ctx.AnimationTimer > 36 || !Ctx.CharacterController.isGrounded)
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

    public IEnumerator LockJump(float delay)
    {
        //Debug.Log("Locking jump");
        Ctx.IsJumpLocked = true;
        yield return new WaitForSeconds(delay);
        Ctx.IsJumpLocked = false;
        //Debug.Log("Unlocking jump");
    }
    public override void InitializeSubState()
    {

    }
}
