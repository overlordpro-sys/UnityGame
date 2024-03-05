using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState {
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) {
    }

    private void Jump() {
        Player.TimerManager.LastPressedJumpTime = 0;
        Player.TimerManager.LastOnGroundTime = 0;
        Player.Body.velocityY = 0;

        Player.Body.AddForceY(Player.MovementData.jumpForce, ForceMode2D.Impulse);
    }

    public override void EnterState() {
        base.EnterState();
        Jump();
        Player.AnimationManager.SetAnimation(PlayerAnimationType.Jump);
        Player.SetGravityScale(Player.MovementData.gravityScale);

    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();
        if (Player.Body.velocityY > 0 && Player.InputScript.JumpAction.action.WasReleasedThisFrame()) {
            StateMachine.ChangeState(Player.FallingState);
            Player.SetGravityScale(Player.MovementData.gravityScale * Player.MovementData.jumpCutGravityMult);
            Player.Body.velocity = new Vector2(Player.Body.velocity.x, Mathf.Min(Player.Body.velocity.y, -Player.MovementData.maxFallSpeed));
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        if (IsJumpHang()) {
            Player.SetGravityScale(Player.MovementData.gravityScale * Player.MovementData.jumpHangGravityMult);
        }
        if (Player.Body.velocityY < 0) {
            StateMachine.ChangeState(Player.FallingState);
        }
    }

    protected override float ModifyAccel(float accelRate) {
        //Increase our acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if (IsJumpHang()) {
            accelRate *= Player.MovementData.jumpHangAccelerationMult;
        }
        return accelRate;
    }

    protected override float ModifyTargetSpeed(float targetSpeed) {
        if (IsJumpHang()) {
            targetSpeed *= Player.MovementData.jumpHangMaxSpeedMult;
        }
        return targetSpeed;
    }


    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
