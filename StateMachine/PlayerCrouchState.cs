using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState, IRootState
{
    public PlayerCrouchState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory ){
        IsRootState = true;
    }

    public override void EnterState()
    {
        Ctx.Animator.SetBool("isCrouching", true);
        InitializeSubState();
        Ctx.CharacterController.height = 1f;
        Ctx.CharacterController.radius = 0.6f;
        Ctx.CharacterController.center = new Vector3(0, 0.5f, 0);
        Ctx._crouchTransposer.m_XAxis.Value = Ctx._normalTransposer.m_XAxis.Value;
        Ctx._crouchPov.m_HorizontalAxis.Value = Ctx._normalPov.m_HorizontalAxis.Value;
        Ctx._crouchCamera.Priority = 9;
        Ctx._normalCamera.Priority = 1;

        


        
    }
    public override void ExitState()
    {
       Ctx.Animator.SetBool("isCrouching", false);
       Ctx.CharacterController.height = 2f;
       Ctx.CharacterController.radius = 0.2f;
       Ctx.CharacterController.center = new Vector3(0, 1f, 0);
        Ctx._normalTransposer.m_XAxis.Value = Ctx._crouchTransposer.m_XAxis.Value;
        Ctx._normalPov.m_HorizontalAxis.Value = Ctx._crouchPov.m_HorizontalAxis.Value;
        Ctx._crouchCamera.Priority = 1;
        Ctx._normalCamera.Priority = 9;
        
    }

    public override void CheckSwitchStates()
    {
       
        if(!Ctx.IsCrouching && CanStandUp())
        {
            SwitchState(Factory.Grounded());
        }
        else if (Ctx.IsJumpPressed && CanStandUp())
        {
            Ctx.IsCrouching = false;
            Ctx.RequireNewJumpPress = true;
            SwitchState(Factory.Grounded());
        }else if (!Ctx.CharacterController.isGrounded && !Ctx.IsJumpPressed)
        {
            SwitchState(Factory.Fall());
        }       
    }

    public bool CanStandUp()
    {
        return !Physics.Raycast(new Vector3(
            Ctx.transform.position.x,
            Ctx.transform.position.y+1f,
            Ctx.transform.position.z),Vector3.up, 1f
        );

    }

    public override void FixedUpdateState()
    {
        
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        //Debug.Log(CanStandUp());
    }

    public override void InitializeSubState()
    {
       if(Ctx.IsMovementPressed)
       {
            SetSubState(Factory.Walk());
       }
       else
       {
            SetSubState(Factory.Idle());
       }
    }
    public void HandleGravity()
    {
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }
}
