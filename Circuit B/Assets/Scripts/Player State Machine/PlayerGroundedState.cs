using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) 
    { 
        //IsRootState = true;
    }
    public override void CheckSwitchStates()
    {
        if (Context.IsJumpPressed && !Context.RequireNewJumpPress)
        {
            SwitchState(Factory.Jump());
        }
        else if (!Context.CharacterController.isGrounded)
        {
            Debug.Log("Switching to falling state");
            SwitchState(Factory.Falling());
        }

        if (Context.IsDead)
        {
            SwitchState(Factory.Dead());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Entering grounded state");
        InitializeSubState();
        HandleGravity();
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
        if(!Context.IsMovementPressed && !Context.IsRunPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if (Context.IsMovementPressed && !Context.IsRunPressed)
        {
            SetSubState(Factory.Walk());
        }
        else
        {
            SetSubState(Factory.Run());
        }
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public void HandleGravity()
    {
        Context.CurrentMovementY = Context.Gravity;
        Context.AppliedMovementY = Context.Gravity;
    }
}
