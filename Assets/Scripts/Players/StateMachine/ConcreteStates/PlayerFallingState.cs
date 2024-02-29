using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerState {
    public PlayerFallingState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) {
    }

    public override void EnterState() {
        Player.SetGravityScale(StateMachine.MovementData.gravityScale * StateMachine.MovementData.fallGravityMult);
        //Player.Body.velocity = new Vector2(Player.Body.velocity.x, Mathf.Min(Player.Body.velocity.y, -StateMachine.MovementData.maxFallSpeed));
        base.EnterState();
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        if (Player.ColliderScript.IsGrounded()) {
            if (Mathf.Abs(Player.Body.velocityX) <= StateMachine.MovementData.stillSpeedThreshold) {
                StateMachine.ChangeState(Player.StillState);
            }
            else {
                StateMachine.ChangeState(Player.RunState);
            }
        }
        if (CanJump() && Player.TimerManager.LastPressedJumpTime > 0) {
            StateMachine.ChangeState(Player.JumpState);
        }
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
