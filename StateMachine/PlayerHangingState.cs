using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState, IRootState
{
    // Start is called before the first frame update
    public PlayerHangingState(PlayerStateMachine currentContext,
                             PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
                             {
                                
                                IsRootState = true;
                                
                             }

    public override void EnterState()
    {
       InitializeSubState();
       Ctx.RequireNewDropPress = true;
       Ctx.Animator.SetBool("isHanging", true);
        //Ctx.AudioManager.Stop($"{Ctx.Surface}Run");
        //Ctx.AudioManager.Stop($"{Ctx.Surface}Walk");
       Ctx.CharacterController.enabled = false;
       //Debug.Log("Enter hanging");
       if(!Ctx.IsCurrentlyDropping)
       {
        Ctx.CharacterController.transform.RotateAround(Ctx.CharacterController.transform.position, Vector3.up, Ctx.LedgeRotation * -1f);
        Ctx.CharacterController.transform.position = Ctx.LedgeCoordinates +Ctx.CharacterController.transform.forward * -0.25f + Ctx.CharacterController.transform.up * -2.2f;
       }

       if(Ctx.IsCurrentlyDropping)
       {
        Ctx.CharacterController.transform.RotateAround(Ctx.CharacterController.transform.position, Vector3.up, 25f);
       }
       
    //    Debug.Log(Ctx.LedgeCoordinates);
    //    Debug.Log(Ctx.CharacterController.transform.position);
       
       
    }
    
    public void HandleGravity()
    {
        Ctx.AppliedMovementY = 0;
    }
    public override void ExitState()
    {
        Ctx.Animator.SetBool("isHanging", false);
        Ctx.Animator.SetBool("isDropping", false);
        Ctx.IsCurrentlyDropping = false;
        Ctx.AppliedMovementY = 0;
        Ctx.CharacterController.enabled = true;
        //Ctx.Attached = false;
        
    }
    public override void FixedUpdateState()
    {
        
    }
    
    public override void UpdateState()
    {
       CheckSwitchStates();
       
    }

    public override void CheckSwitchStates()
    {
        if(Ctx.LetGo && !Ctx.RequireNewDropPress)
        {
            Ctx.IsHanging = false;
            Ctx.LetGo = false;
            Ctx.IsLanding = true;
            Ctx.CharacterController.enabled = true;
            
            
            

            SwitchState(Factory.Fall());
        }
        else if(Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress)
        {
            Ctx.IsClimbing = true;
            Ctx.IsHanging = false;
            SwitchState(Factory.Climb());
        }
    }

    public override void InitializeSubState()
    {
        if(Ctx.IsRunPressed && Ctx.IsMovementPressed)
        {
            SetSubState(Factory.Run());
        }
        else if (Ctx.IsMovementPressed)
        {
            SetSubState(Factory.Walk());
        }
        else
        {
            SetSubState(Factory.Idle());
        }    

        //SetSubState(Factory.Idle());
        
    }
}
