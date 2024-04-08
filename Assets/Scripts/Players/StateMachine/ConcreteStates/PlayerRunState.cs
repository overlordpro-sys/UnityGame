using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState {
    public PlayerRunState(PlayerBase playerBase, PlayerStateMachine stateMachine) : base(playerBase, stateMachine) {
    }
    public override void EnterState() {
        base.EnterState();
        PlayerBase.AnimationManager.SetAnimation(PlayerAnimationType.Run);
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();
        PlayerBase.TimerManager.LastOnGroundTime = PlayerBase.MovementData.coyoteTime;
        if (Mathf.Abs(PlayerBase.Body.velocityX) <= PlayerBase.MovementData.stillSpeedThreshold && PlayerBase.InputScript.MoveDirection.x == 0) {
            StateMachine.ChangeState(PlayerBase.IdleState);
        }
        if (CanJump() && PlayerBase.TimerManager.LastPressedJumpTime > 0) {
            StateMachine.ChangeState(PlayerBase.JumpState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        if (PlayerBase.Body.velocityY < -3) {
            StateMachine.ChangeState(PlayerBase.FallingState);
        }
    }

    public override void AnimationTriggerEvent(PlayerBase.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
