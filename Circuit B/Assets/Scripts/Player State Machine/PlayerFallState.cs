using UnityEngine;

public class PlayerFallState : PlayerBaseState, IRootState
{
    public PlayerFallState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        //IsRootState = true;
    }

    public override void CheckSwitchStates()
    {
        if (Context.CharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
        Context.Animator.SetBool(Context.IsFallingHash, true);
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsFallingHash, false);
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

    public void HandleGravity()
    {
        float previousYVelocity = Context.AppliedMovementY;
        Context.CurrentMovementY = Context.CurrentMovementY + Context.Gravity * Time.deltaTime;
        Context.AppliedMovementY = Mathf.Max((previousYVelocity + Context.CurrentMovementY) * .5f, Context.MaxFallVelocity);
    }
}
