using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerClimbingState : PlayerBaseState, IRootState
{
    public PlayerClimbingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }
    public void HandleGravity()
    {
        Ctx.AppliedMovementY = 0;
    }
    public override void EnterState()
    {
        InitializeSubState();
        Ctx.AppliedMovement = Vector3.zero;
        Ctx.Animator.SetBool("isClimbing", true);
        Ctx.AudioManager.Play("roll");
        Ctx.CharacterController.enabled = false;
        //Ctx.CharacterController.enabled = false;
        //Ctx.CharacterController.transform.position = Vector3.MoveTowards(Ctx.CharacterController.transform.position, Ctx.CharacterController.transform.position+Ctx.transform.forward * 0.4f, 0.1f);
        Ctx.AnimationTimer = 0;
        Ctx.Brain.ActiveVirtualCamera.VirtualCameraGameObject.TryGetComponent<CinemachineCollider>(out var collider);
        if (collider)
        {
            collider.m_DampingWhenOccluded = 1;
        }
        //Debug.Log("Enter climbing");
    }
    public override void ExitState()
    {
        
        Ctx.IsClimbing = false;
        Ctx.Animator.SetBool("isClimbing", false);
        //Ctx.Animator.SetBool("isWalking", false);
        //Ctx.Animator.SetBool("isRunning", false);
        //Debug.Log("Exit climbing");
        Ctx.CharacterController.enabled = true;
        Ctx.AnimationTimer = 0;
        Ctx.Brain.ActiveVirtualCamera.VirtualCameraGameObject.TryGetComponent<CinemachineCollider>(out var collider);
        if (collider)
        {
            collider.m_DampingWhenOccluded = 0;
        }
        //Ctx.Attached = false;
        
    }
    public override void FixedUpdateState()
    {
        Ctx.AnimationTimer++;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        
        
        //Debug.Log(Ctx.AnimationTimer);

    }
    
    public override void CheckSwitchStates()
    {
        if(Ctx.AnimationTimer > 125)
        {
            //Ctx.CharacterController.transform.position = Ctx.LedgeCoordinates;
            //Ctx.CharacterController.enabled = true;
            SwitchState(Factory.Grounded());
            //Debug.Log("Swith state");
        }
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
    //SetSubState(Factory.Idle());
    }
}
