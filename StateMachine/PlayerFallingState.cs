using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState, IRootState
{
    public PlayerFallingState(PlayerStateMachine currentContext,
                               PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {

        IsRootState = true;
    }
    public override void EnterState()
    {
        Ctx.IsFalling = true;
        InitializeSubState();
        HandleGravity();
        Ctx.Animator.SetBool("isFalling", true);
        


    }
    public override void ExitState()
    {
        Ctx.IsFalling = false;
        Ctx.Animator.SetBool("isFalling", false);
        Ctx.Animator.SetBool("ledgeBelow", false);
        if (Ctx.IsHanging)
        {
            Ctx.Animator.SetBool("isHanging", true);
        }
        else if (Ctx.IsMovementPressed)
        {
            Ctx.IsRolling = true;
            Ctx.IsLanding = false;
            Ctx.Animator.SetBool("isRolling", true);
            Ctx.IsJumpLocked = true;
            Ctx.JumpLockTimer = 0;


        }
        else
        {
            Ctx.IsLanding = true;
            Ctx.IsRolling = false;
            //Debug.Log("Landing choice");
            Ctx.Animator.SetBool("isLanding", true);
            //Debug.Log("Landing anim bool true");
            Ctx.IsJumpLocked = true;
            Ctx.JumpLockTimer = 0;
            //Debug.Log("Jump Locked");
        }




        //Ctx.Animator.SetBool("isFalling", false);

    }
    public override void FixedUpdateState()
    {
        
    }
    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();
        Ctx.CheckForLedge();
        CheckForLedgeBelow();
    }

    

    public void HandleGravity()
    {
        float previousYVelocity = Ctx.CurrentMovementY;
        Ctx.CurrentMovementY = Ctx.CurrentMovementY + Ctx.Gravity * Time.deltaTime;
        Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * .5f, -7.0f); // -15f
    }

    public void CheckForLedgeBelow()
    {
        if (Ctx.AppliedMovementY <= 0)
        {
            if (Physics.Raycast(Ctx.CharacterController.transform.position + Ctx.CharacterController.transform.forward * 0.5f, Vector3.down, out var verticalHit))
            {
                if (verticalHit.distance < 4f)
                {
                    if (Physics.Raycast(Ctx.CharacterController.transform.position + Vector3.down * verticalHit.distance, Ctx.CharacterController.transform.forward, out var horizontalHit))
                    {
                        if (horizontalHit.distance < 0.5f && horizontalHit.normal.y == 0)
                        {
                            Ctx.LedgeBelow = true;
                            Ctx.Animator.SetBool("ledgeBelow", true);
                            return;

                        }
                        

                    }
                }

            }
        }
        Ctx.LedgeBelow = false;

    }
    public override void CheckSwitchStates()
    {
        if (Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Ctx.IsHanging)
        {
            Ctx.AppliedMovement = Vector3.zero;
            SwitchState(Factory.Hang());
        }
    }
    public override void InitializeSubState()
    {
        if (Ctx.IsRolling)
        {
            SetSubState(Factory.Roll());
        }
        else if (Ctx.IsLanding)
        {
            SetSubState(Factory.Land());
        }
        else if (Ctx.IsMovementPressed && Ctx.IsRunPressed)
        {
            
            SetSubState(Factory.Run());
        }
        else if (!Ctx.IsMovementPressed)
        {
            SetSubState(Factory.Walk());
        }
        else
        {
            SetSubState(Factory.Idle());
        }
    }
}
