using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState, IRootState
{

    int letGoCounter = 0;
    
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
        letGoCounter = 0;


    }
    public override void ExitState()
    {
        Ctx.IsFalling = false;
        Ctx.Animator.SetBool("isFalling", false);
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
        CheckForLedge();
    }

    public void CheckForLedge()
    {

        if (Ctx.AppliedMovementY <= 0)
        {
            //chaing 1.0f to 0.5f in transform.forward and 2.5f to 2.0f in transform.up
            if (Physics.Raycast(Ctx.CharacterController.transform.position + Ctx.CharacterController.transform.forward * 0.5f + Ctx.CharacterController.transform.up * 2.0f, Vector3.down, out var verticalHit))
            {

                if (verticalHit.distance < 0.2f)
                {
                    //Debug.DrawRay(Ctx.CharacterController.transform.position + Ctx.CharacterController.transform.forward * 0.5f + Ctx.CharacterController.transform.up * 2.0f, Vector3.down,Color.green, 500);
                    //Debug.DrawRay(new Vector3(Ctx.CharacterController.transform.position.x, verticalHit.point.y - 0.2f, Ctx.CharacterController.transform.position.z), Ctx.CharacterController.transform.forward, Color.yellow, 500f);
                    if (Physics.Raycast(new Vector3(Ctx.CharacterController.transform.position.x, verticalHit.point.y - 0.01f, Ctx.CharacterController.transform.position.z), Ctx.CharacterController.transform.forward, out var horizontalHit))
                    //if (Physics.Raycast(new Vector3(Ctx.CharacterController.transform.position.x, verticalHit.point.y - 0.01f, Ctx.CharacterController.transform.position.z) + Ctx.CharacterController.transform.forward * -1f, Ctx.CharacterController.transform.forward, out var horizontalHit))
                    {
                        if (horizontalHit.distance < 0.5f && horizontalHit.normal.y == 0)
                        {

                            Vector3 edgeRotation = horizontalHit.normal;
                            //Debug.DrawRay(horizontalHit.point, horizontalHit.normal, Color.magenta, 500);
                            //Debug.DrawRay(Ctx.CharacterController.transform.position, Ctx.CharacterController.transform.forward * -1f, Color.blue, 500);
                            Vector3 edgePosition = new Vector3(horizontalHit.point.x, verticalHit.point.y, horizontalHit.point.z);
                            float rotationNeeded = Vector3.SignedAngle(edgeRotation, Ctx.CharacterController.transform.forward * -1f, Vector3.up);
                            //Debug.Log($"Normal:{horizontalHit.normal}");
                            Ctx.LedgeCoordinates = edgePosition;
                            Ctx.IsHanging = true;
                            if (verticalHit.collider.gameObject.layer == 8)
                            {

                                // if (!Ctx.Attached)
                                // {
                                //     Ctx.PreviousParent = Ctx.CharacterController.transform.parent;
                                // }
                                Ctx.Attached = true;
                                //Ctx.PreviousParent = Ctx.CharacterController.transform.parent;
                                Ctx.CharacterController.transform.parent = verticalHit.transform.parent;
                            }
                            Ctx.LedgeRotation = rotationNeeded;

                        }

                    }
                }

            }
        }
    }

    public void HandleGravity()
    {
        float previousYVelocity = Ctx.CurrentMovementY;
        Ctx.CurrentMovementY = Ctx.CurrentMovementY + Ctx.Gravity * Time.deltaTime;
        Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * .5f, -7.0f); // -15f
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
