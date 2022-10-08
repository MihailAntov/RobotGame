using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public int _delayCounter = 0;

    public PlayerGroundedState(PlayerStateMachine currentContext,
                             PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {

        IsRootState = true;

    }

    public override void EnterState()
    {
        //Debug.Log("Enter grounded");
        InitializeSubState();
        //Debug.Log("Grounded substate chosen");
        HandleGravity();

        if (Ctx.IsLooking)
        {
            if (!Ctx.InLookState)
            {
                Ctx.InLookState = true;
                Ctx._crosshair.SetActive(true);
                //Ctx.IsLooking = false;
                
                
                Ctx._lookCamera.Priority = 9;
                Ctx._normalCamera.Priority = 1;
                Ctx._lookPov.m_HorizontalAxis.Value = Ctx._normalPov.m_HorizontalAxis.Value;
                Ctx._lookTransposer.m_XAxis.Value = Ctx._normalTransposer.m_XAxis.Value;
                Ctx.HandleStrafeAnimations();
            }
        }

        //set footsteps
        //------------------------------
        // if(!Ctx.IsRolling)
        // {
        //     Ctx.footstep.volume = 1;
        // }

        //---------------------------
    

    }
    public void HandleGravity()
    {
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }

    public bool CanDrop()
    {
        //Debug.DrawRay(Ctx.CharacterController.transform.position + Ctx.CharacterController.transform.forward * 1f, Vector3.down, Color.red, 50);
        if (Physics.Raycast(Ctx.CharacterController.transform.position + Ctx.CharacterController.transform.forward * 1f, Vector3.down, out var verticalHit))
        {
            if (verticalHit.distance > 2.0f)
            {
                //Debug.DrawRay(Ctx.CharacterController.transform.position + Ctx.CharacterController.transform.forward * 1f + Ctx.CharacterController.transform.up * -0.1f, Ctx.CharacterController.transform.forward * -1f, Color.red, 50);
                if (Physics.Raycast(Ctx.CharacterController.transform.position + Ctx.CharacterController.transform.forward * 1f + Ctx.CharacterController.transform.up * -0.1f, Ctx.CharacterController.transform.forward * -1f, out var horizontalHit))
                //if (Physics.Raycast(new Vector3(Ctx.CharacterController.transform.position.x, verticalHit.point.y - 0.01f, Ctx.CharacterController.transform.position.z) + Ctx.CharacterController.transform.forward * -1f, Ctx.CharacterController.transform.forward, out var horizontalHit))
                {
                    if (horizontalHit.distance < 1f)
                    {
                        Vector3 edgeRotation = horizontalHit.normal;
                        Vector3 edgePosition = new Vector3(horizontalHit.point.x, Ctx.CharacterController.transform.position.y - 0.1f, horizontalHit.point.z);
                        float rotationNeeded = Vector3.SignedAngle(Ctx.CharacterController.transform.forward, edgeRotation, Vector3.up);
                        //Debug.Log(rotationNeeded);
                        Ctx.LedgeCoordinates = edgePosition;
                        Ctx.LedgeRotation = rotationNeeded;
                        return true;

                    }

                }
            }

        }

        return false;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState()
    {
        if (Ctx.InLookState)
        {
            Ctx.InLookState = false;
            Ctx._crosshair.SetActive(false);
            //Ctx.IsLooking = false;
            Ctx._lookCamera.Priority = 1;
            Ctx._normalCamera.Priority = 9;
            Ctx._normalPov.m_HorizontalAxis.Value = Ctx._lookPov.m_HorizontalAxis.Value;
            Ctx._normalTransposer.m_XAxis.Value = Ctx._lookTransposer.m_XAxis.Value;
            Ctx.Animator.SetBool("isWalkingBack", false);
            Ctx.Animator.SetBool("isStrafing", false);
            Ctx.Animator.SetBool("isStrafingLeft", false);
            Ctx.Animator.SetBool("isStrafingRight", false);
            Ctx.Animator.SetBool("isStrafingLeftDiag", false);
            Ctx.Animator.SetBool("isStrafingRightDiag", false);
            Ctx.Animator.SetBool("isStrafingLeftDiagF", false);
            Ctx.Animator.SetBool("isStrafingRightDiagF", false);
        }
        Ctx.footstep.volume = 0;

        Ctx.CharacterController.transform.parent = Ctx.PreviousParent;
        Ctx.Attached = false;
    }
    public override void FixedUpdateState()
    {

    }
    public override void InitializeSubState()
    {

        // if (Ctx.IsRolling)
        // {
        //     SetSubState(Factory.Roll());
        // }
        // else if (Ctx.IsLanding)
        // {
        //     SetSubState(Factory.Land());
        // }
        // else 
        if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Walk());
        }
        else
        {
            SetSubState(Factory.Run());
        }


    }
    public override void CheckSwitchStates()
    {
        if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress && !Ctx.IsHanging && !Ctx.IsRolling && !Ctx.IsLanding)
        {
            SwitchState(Factory.Jump());
        }
        else if (Ctx.IsDropping && !Ctx.RequireNewDropPress)
        {
            Ctx.RequireNewDropPress = true;
            SwitchState(Factory.Drop());
        }
        else if (Ctx.IsCrouching && Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Crouch());
        }
        else if (!Ctx.CharacterController.isGrounded && !Ctx.IsJumpPressed && !Ctx.IsHanging && !Ctx.IsRolling)
        {
            if (_delayCounter < 20)
            {
                _delayCounter++;
            }
            else
            {
                _delayCounter = 0;
                SwitchState(Factory.Fall());
            }
        }
        else if (Ctx.IsRolling && !Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Fall());
        }
        else
        {
            _delayCounter = 0;
        }
    }


}
