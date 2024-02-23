using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStillState : PlayerState {
    public PlayerStillState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) {
    }

    public override void EnterState() {
        base.EnterState();
        Player.SetGravityScale(StateMachine.MovementData.gravityScale);

    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();
        Player.TimerManager.LastOnGroundTime = StateMachine.MovementData.coyoteTime;
        if (Player.InputScript.MoveDirection.x != 0) {
            StateMachine.ChangeState(Player.RunState);
        }
        if (CanJump() && Player.InputScript.JumpAction.action.WasPressedThisFrame()) {
            StateMachine.ChangeState(Player.JumpState);
        }

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
