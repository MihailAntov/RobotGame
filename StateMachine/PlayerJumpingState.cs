using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState, IRootState
{
   int jumpTimer = 0;
   
   public PlayerJumpingState(PlayerStateMachine currentContext, 
                              PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
                              {
                                
                                IsRootState = true;
                              }
   public override void EnterState()
    {
       
       InitializeSubState();
        HandleJump();
        jumpTimer = 0;
        
        Ctx.AudioManager.Play("jump");
       
    }
    public override void UpdateState()
    {
        HandleGravity();
        CheckSwitchStates();   
        CheckForLedge();
    }
    public override void ExitState()
    {
        //Ctx.Animator.SetBool("isJumping", false);
        Ctx.Animator.SetBool(Ctx.IsJumpingHash, false);
        if(Ctx.IsJumpPressed)
        {
            Ctx.RequireNewJumpPress = true;
        }
        Ctx.IsJumping = false;

        if(Ctx.CharacterController.isGrounded && jumpTimer > 80)
        {
            if(Ctx.IsMovementPressed)
            {
                Ctx.IsRolling  = true;
                Ctx.Animator.SetBool("isRolling", true);
            }
            else 
            {
                Ctx.IsLanding = true;
                Ctx.Animator.SetBool("isLanding", true);
            }
            jumpTimer = 0;
        }
        
        
    }
    public override void FixedUpdateState()
    {
        jumpTimer++;
    }
    public override void InitializeSubState()
    {
        if(!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if(Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Walk());
        }else
        {
            SetSubState(Factory.Run());
        }
    }
    public override void CheckSwitchStates()
    {
        if(Ctx.IsHanging)
        {
            Ctx.AppliedMovement = Vector3.zero;
            SwitchState(Factory.Hang());
        }
        else if (Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        } else if (Ctx.AppliedMovementY < -10f )
        {
            SwitchState(Factory.Fall());
        }
    }

    void HandleJump()
    {
        
        Ctx.Animator.SetBool(Ctx.IsJumpingHash, true);
        Ctx.IsJumping = true;
        Ctx.CurrentMovementY = Ctx.InitialJumpVelocity;
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocity;
        
        
        
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
                    if (Physics.Raycast(new Vector3(Ctx.CharacterController.transform.position.x, verticalHit.point.y - 0.01f, Ctx.CharacterController.transform.position.z) , Ctx.CharacterController.transform.forward, out var horizontalHit))
                    //if (Physics.Raycast(new Vector3(Ctx.CharacterController.transform.position.x, verticalHit.point.y - 0.01f, Ctx.CharacterController.transform.position.z) + Ctx.CharacterController.transform.forward * -1f, Ctx.CharacterController.transform.forward, out var horizontalHit))
                    {
                        if (horizontalHit.distance < 0.5f && horizontalHit.normal.y == 0) 
                        {
                            
                            Vector3 edgeRotation = horizontalHit.normal;
                            Vector3 edgePosition = new Vector3(horizontalHit.point.x, verticalHit.point.y, horizontalHit.point.z);
                            float rotationNeeded = Vector3.SignedAngle(edgeRotation, Ctx.CharacterController.transform.forward * -1f, Vector3.up);
                            Ctx.LedgeCoordinates = edgePosition;
                            Ctx.IsHanging = true;
                            //Debug.Log(verticalHit.collider.gameObject.layer);
                            if(verticalHit.collider.gameObject.layer == 8)
                            {
                                // if(!Ctx.Attached)
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
        bool isFalling = Ctx.CurrentMovementY <= 0.0f || !Ctx.IsJumpPressed;
        float fallMultiplier = 1f;
        if (isFalling)
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.Gravity * fallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * 0.5f, -15.0f);
            
        }
        else
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.Gravity * Time.deltaTime);
            Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentMovementY) * 0.5f;
            
        }
    }
}
