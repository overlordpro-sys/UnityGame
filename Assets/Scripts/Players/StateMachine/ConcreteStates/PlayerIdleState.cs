using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState {
    public PlayerIdleState(PlayerBase playerBase, PlayerStateMachine stateMachine) : base(playerBase, stateMachine) {
    }

    public override void EnterState() {
        base.EnterState();
        PlayerBase.AnimationManager.SetAnimation(PlayerAnimationType.Idle);
        PlayerBase.SetGravityScale(PlayerBase.MovementData.gravityScale);

    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();
        PlayerBase.TimerManager.LastOnGroundTime = PlayerBase.MovementData.coyoteTime;
        if (PlayerBase.InputScript.MoveDirection.x != 0) {
            StateMachine.ChangeState(PlayerBase.RunState);
        }
        if (CanJump() && PlayerBase.TimerManager.LastPressedJumpTime > 0) {
            StateMachine.ChangeState(PlayerBase.JumpState);
        }

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

    }

    public override void AnimationTriggerEvent(PlayerBase.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
