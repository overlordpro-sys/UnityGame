using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerState {
    public PlayerFallingState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) {
    }

    public override void EnterState() {
        Player.SetGravityScale(Player.MovementData.gravityScale * Player.MovementData.fallGravityMult);
        Player.AnimationManager.SetAnimation(PlayerAnimationType.Fall);
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
            if (Mathf.Abs(Player.Body.velocityX) <= Player.MovementData.stillSpeedThreshold) {
                StateMachine.ChangeState(Player.IdleState);
            }
            else {
                StateMachine.ChangeState(Player.RunState);
            }
        }
        if (CanJump() && Player.TimerManager.LastPressedJumpTime > 0) {
            StateMachine.ChangeState(Player.JumpState);
        }
    }

    protected override float ModifyAccel(float accelRate) {
        //Increase our acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if (Mathf.Abs(Player.Body.velocityY) < Player.MovementData.jumpHangTimeThreshold) {
            accelRate *= Player.MovementData.jumpHangAccelerationMult;
        }
        return accelRate;
    }

    protected override float ModifyTargetSpeed(float targetSpeed) {
        if (Mathf.Abs(Player.Body.velocityY) < Player.MovementData.jumpHangTimeThreshold) {
            targetSpeed *= Player.MovementData.jumpHangMaxSpeedMult;
        }
        return targetSpeed;
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
