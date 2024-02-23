using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState {
    public PlayerRunState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) {
    }
    public override void EnterState() {
        base.EnterState();
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();
        Player.TimerManager.LastOnGroundTime = StateMachine.MovementData.coyoteTime;
        if (Mathf.Abs(Player.Body.velocityX) <= StateMachine.MovementData.stillSpeedThreshold && Player.InputScript.MoveDirection.x == 0) {
            StateMachine.ChangeState(Player.StillState);
        }
        if (CanJump() && Player.InputScript.JumpAction.action.WasPressedThisFrame()) {
            StateMachine.ChangeState(Player.JumpState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        if (Player.Body.velocityY < 0) {
            StateMachine.ChangeState(Player.FallingState);
        }
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
