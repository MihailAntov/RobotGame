using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{
    public PlayerRunningState(PlayerStateMachine currentContext,
                             PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
    public override void EnterState()
    {
        Ctx.Animator.SetBool("isWalking", true);
        Ctx.Animator.SetBool("isRunning", true);

        // Ctx.footstep.pitch = 1f;
        // Ctx.footstep.volume = 1;
        // Ctx.footstep.Play();

        if(Ctx.Surface != "none")
        {
            Ctx.AudioManager.Play($"{Ctx.Surface}Run");
            Ctx.Runsteps = Ctx.Surface;
        }
        



    }
    public override void UpdateState()
    {

        if (!Ctx.IsHanging && !Ctx.IsClimbing)
        {

            Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * Ctx.RunSpeed;
            Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.RunSpeed;
        }


        if (Ctx.IsJumping)
        {
            Ctx.AppliedMovementX *= 1.15f;
            Ctx.AppliedMovementZ *= 1.15f;
        }
        Ctx.AppliedMovement = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f) * Ctx.AppliedMovement;


        //lock jump after roll
        if (Ctx.JumpLockTimer > 5)
        {
            Ctx.IsJumpLocked = false;
            Ctx.JumpLockTimer = 0;
            //Debug.Log("Jump unlocked");
        }



        //CheckSwitchStates();
    }
    public override void ExitState()
    {
        //Debug.Log("Exit running");
        //Ctx.footstep.Stop();
        StopSteps();
    }

    public void StopSteps()
    {
        foreach (string surface in Ctx.Surfaces)
        {
            Ctx.AudioManager.Stop($"{surface}Run");
            Ctx.AudioManager.Stop($"{surface}Step");
        }
    }
    public override void FixedUpdateState()
    {
        CheckSwitchStates();
        if (Ctx.IsJumpLocked)
        {
            Ctx.JumpLockTimer++;
        }
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        if(Ctx.IsLanding || Ctx.IsRolling)
        {
            return;
        }
        
        Ctx.DetectSurface();
        

        if (Ctx.Surface != Ctx.Runsteps)
        {
            StopSteps();
            if (Ctx.Surface != "none")
            {
                Ctx.AudioManager.Play($"{Ctx.Surface}Run");
                Ctx.Runsteps = Ctx.Surface;
            }

        }
    }

    public override void InitializeSubState()
    {

    }
    public override void CheckSwitchStates()
    {
        if (Ctx.IsRolling)
        {
            SwitchState(Factory.Roll());
        }
        else if (Ctx.IsLanding)
        {
            SwitchState(Factory.Land());
        }
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SwitchState(Factory.Walk());
        }
        else if (!Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        }
    }
}
