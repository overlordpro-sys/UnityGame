using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState {
    public PlayerJumpState(PlayerBase playerBase, PlayerStateMachine stateMachine) : base(playerBase, stateMachine) {
    }

    private void Jump() {
        PlayerBase.TimerManager.LastPressedJumpTime = 0;
        PlayerBase.TimerManager.LastOnGroundTime = 0;
        PlayerBase.Body.velocityY = 0;

        PlayerBase.Body.AddForceY(PlayerBase.MovementData.jumpForce, ForceMode2D.Impulse);
    }

    public override void EnterState() {
        base.EnterState();
        Jump();
        PlayerBase.AnimationManager.SetAnimation(PlayerAnimationType.Jump);
        PlayerBase.SetGravityScale(PlayerBase.MovementData.gravityScale);

    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();
        if (PlayerBase.Body.velocityY > 0 && PlayerBase.InputScript.JumpAction.action.WasReleasedThisFrame()) {
            StateMachine.ChangeState(PlayerBase.FallingState);
            PlayerBase.SetGravityScale(PlayerBase.MovementData.gravityScale * PlayerBase.MovementData.jumpCutGravityMult);
            PlayerBase.Body.velocity = new Vector2(PlayerBase.Body.velocity.x, Mathf.Min(PlayerBase.Body.velocity.y, -PlayerBase.MovementData.maxFallSpeed));
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        if (IsJumpHang()) {
            PlayerBase.SetGravityScale(PlayerBase.MovementData.gravityScale * PlayerBase.MovementData.jumpHangGravityMult);
        }
        if (PlayerBase.Body.velocityY < 0) {
            StateMachine.ChangeState(PlayerBase.FallingState);
        }
    }

    protected override float ModifyAccel(float accelRate) {
        //Increase our acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if (IsJumpHang()) {
            accelRate *= PlayerBase.MovementData.jumpHangAccelerationMult;
        }
        return accelRate;
    }

    protected override float ModifyTargetSpeed(float targetSpeed) {
        if (IsJumpHang()) {
            targetSpeed *= PlayerBase.MovementData.jumpHangMaxSpeedMult;
        }
        return targetSpeed;
    }


    public override void AnimationTriggerEvent(PlayerBase.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
