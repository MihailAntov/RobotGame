using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerWalkingState : PlayerBaseState
{
    public PlayerWalkingState(PlayerStateMachine currentContext, 
                                PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory){}
    
    public override void EnterState()
    {
        Ctx.Animator.SetBool("isWalking",true);
        Ctx.Animator.SetBool("isRunning",false);
        //------------------ previous footstep implementation --------------
        //Ctx.footstep.pitch = 0.6f;
        //Ctx.footstep.volume = 1;
        //Ctx.footstep.Play();

        
         Ctx.AudioManager.Play($"{Ctx.Surface}Step");
         Ctx.Footsteps = Ctx.Surface;
        
        
        
    }
    public override void UpdateState()
    {
        if(!Ctx.IsHanging && !Ctx.IsClimbing && !Ctx.IsUsingTerminal )
        {
            Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * Ctx.WalkSpeed;
            Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.WalkSpeed; 
        }
        
        if (Ctx.IsJumping)
        {
            Ctx.AppliedMovementX *= 1.15f;
            Ctx.AppliedMovementZ *= 1.15f;
        }

        if(Ctx.IsCrouching)
        {
            Ctx.AppliedMovementX *= 0.75f;
            Ctx.AppliedMovementZ *= 0.75f;
        }
        Ctx.AppliedMovement = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f) * Ctx.AppliedMovement;
        
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
       //Ctx.Animator.SetBool("isWalking",false);
       //Debug.Log("Exit walking");
       
       //-------- previous implementation of step
       //Ctx.footstep.Stop();


        StopSteps();

       
    }
    public void StopSteps()
    {
        foreach (string surface in Ctx.Surfaces)
        {
            Ctx.AudioManager.Stop($"{surface}Step");
            Ctx.AudioManager.Stop($"{surface}Run");
        }
    }
    public override void FixedUpdateState()
    {
        CheckSwitchStates();
        if(Ctx.IsJumpLocked)
        {
            Ctx.JumpLockTimer++;
        }
        Ctx.DetectSurface();

        if (Ctx.Surface != Ctx.Footsteps)
        {
            StopSteps();
            Ctx.AudioManager.Play($"{Ctx.Surface}Step");
            Ctx.Footsteps = Ctx.Surface;
        }
    }
    public override void InitializeSubState()
    {
        
    }
    public override void CheckSwitchStates()
    {
        if(Ctx.IsRolling)
        {
            SwitchState(Factory.Roll());
        }
        else if (Ctx.IsLanding)
        {
            SwitchState(Factory.Land());
        }
        else if(Ctx.IsMovementPressed && Ctx.IsRunPressed && !Ctx.IsCrouching)
        {
            SwitchState(Factory.Run());
        }
        else if(!Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        }
    }
}
