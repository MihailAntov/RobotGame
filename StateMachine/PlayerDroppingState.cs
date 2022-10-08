using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDroppingState : PlayerBaseState, IRootState
{
    public PlayerDroppingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }
    public override void EnterState()
    {
        InitializeSubState();
        Ctx.CharacterController.enabled = false;
        Ctx.IsCurrentlyDropping = true;
        Ctx.AnimationTimer = 0;
        Ctx.CharacterController.transform.RotateAround(Ctx.CharacterController.transform.position, Vector3.up, Ctx.LedgeRotation);
        Ctx.CharacterController.transform.position = Ctx.LedgeCoordinates + Ctx.CharacterController.transform.forward * -0.1f + Ctx.CharacterController.transform.up * -0.1f;
        
    }
    public override void ExitState()
    {
        Ctx.AnimationTimer = 0;
        Ctx.IsDropping = false;
        // Ctx.CharacterController.transform.position = Ctx.LedgeCoordinates +Ctx.CharacterController.transform.forward * -0.25f + Ctx.CharacterController.transform.up * -2.2f;
        // Ctx.CharacterController.transform.RotateAround(Ctx.CharacterController.transform.position, Vector3.up, Ctx.LedgeRotation);
        Ctx.IsHanging = true;
        Ctx.CharacterController.enabled = true;

    }
    public override void CheckSwitchStates()
    {
        if (Ctx.AnimationTimer > 100)
        {
            SwitchState(Factory.Hang());
        }
    }
    public override void UpdateState()
    {
        if(Ctx.AnimationTimer > 10)
        {
            Ctx.Animator.SetBool("isDropping", true);
        }
        CheckSwitchStates();
    }
    public override void FixedUpdateState()
    {
        Ctx.AnimationTimer++;
    }
    public override void InitializeSubState()
    {
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
    public void HandleGravity()
    {
        Ctx.AppliedMovementY = 0;
    }
    
}
