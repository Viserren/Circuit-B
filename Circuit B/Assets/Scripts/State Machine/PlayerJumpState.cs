using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        //IsRootState = true;
    }
    public override void CheckSwitchStates()
    {
        if (Context.CharacterController.isGrounded)
        {
            Debug.Log("Switching to grounded state");
            SwitchState(Factory.Grounded());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
        HandleJump();
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsJumpingHash, false);
        if (Context.IsJumpPressed)
        {
            Context.RequireNewJumpPress = true;
        }
        Context.CurrentJumpResetRoutine = Context.StartCoroutine(IJumpResetRoutine());
        if (Context.JumpCount == 3)
        {
            Context.JumpCount = 0;
            Context.Animator.SetInteger(Context.JumpCountHash, Context.JumpCount);
        }
    }

    public override void InitializeSubState()
    {
        if (!Context.IsMovementPressed && !Context.IsRunPressed)
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
        HandleGravity();
        CheckSwitchStates();
    }

    void HandleJump()
    {
        Debug.Log("Handling jump");
        if (Context.JumpCount < 3 && Context.CurrentJumpResetRoutine != null)
        {
            Context.StopCoroutine(Context.CurrentJumpResetRoutine);
        }
        Context.Animator.SetBool(Context.IsJumpingHash, true);
        //Context.IsJumping = true;
        Context.JumpCount++;
        Context.Animator.SetInteger(Context.JumpCountHash, Context.JumpCount);
        Context.CurrentMovementY = Context.InitialJumpVelocities[Context.JumpCount];
        Context.AppliedMovementY = Context.InitialJumpVelocities[Context.JumpCount];
    }

    public void HandleGravity()
    {
        bool isFalling = Context.CurrentMovementY <= 0 || !Context.IsJumpPressed;
        float fallMultiplier = 2;

        if (isFalling)
        {
            float previousYVelocity = Context.CurrentMovementY;
            Context.CurrentMovementY = Context.CurrentMovementY + (Context.JumpGravities[Context.JumpCount] * fallMultiplier * Time.deltaTime);
            Context.AppliedMovementY = Mathf.Max((previousYVelocity + Context.CurrentMovementY) * .5f, Context.MaxFallVelocity);
        }
        else
        {
            float previousYVelocity = Context.CurrentMovementY;
            Context.CurrentMovementY = Context.CurrentMovementY + (Context.JumpGravities[Context.JumpCount] * Time.deltaTime);
            Context.AppliedMovementY = (previousYVelocity + Context.CurrentMovementY) * .5f;
        }
    }

    IEnumerator IJumpResetRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        Context.JumpCount = 0;
    }
}
