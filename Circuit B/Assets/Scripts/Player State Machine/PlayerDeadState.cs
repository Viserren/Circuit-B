using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState, IRootState
{
    public PlayerDeadState(PlayerStateManager currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchStates()
    {
        if (!Context.IsDead)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Die");
        Context.Animator.SetTrigger(Context.ShouldDieHash);
        Context.AppliedMovementX = 0;
        Context.AppliedMovementZ = 0;
    }

    public override void ExitState()
    {
    }

    public void HandleGravity()
    {
        Context.CurrentMovementY = Context.Gravity;
        Context.AppliedMovementY = Context.Gravity;
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        Context.AppliedMovementX = 0;
        Context.AppliedMovementZ = 0;
        CheckSwitchStates();
    }
}
