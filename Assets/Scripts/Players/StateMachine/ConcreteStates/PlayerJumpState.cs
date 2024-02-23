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

        Player.Body.AddForceY(StateMachine.MovementData.jumpForce, ForceMode2D.Impulse);
    }

    public override void EnterState() {
        base.EnterState();
        Jump();
        Player.SetGravityScale(StateMachine.MovementData.gravityScale);

    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();
        if (Player.Body.velocityY > 0 && Player.InputScript.JumpAction.action.WasReleasedThisFrame()) {
            StateMachine.ChangeState(Player.FallingState);
            Player.SetGravityScale(StateMachine.MovementData.gravityScale * StateMachine.MovementData.jumpCutGravityMult);
            Player.Body.velocity = new Vector2(Player.Body.velocity.x, Mathf.Min(Player.Body.velocity.y, -StateMachine.MovementData.maxFallSpeed));
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        if (Player.Body.velocityY < 0) {
            StateMachine.ChangeState(Player.FallingState);
        }
    }

    protected override float ModifyAccel(float accelRate) {
        //Increase our acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if (Player.Body.velocityY < StateMachine.MovementData.jumpHangTimeThreshold) {
            accelRate *= StateMachine.MovementData.jumpHangAccelerationMult;
        }
        return accelRate;
    }

    protected override float ModifyTargetSpeed(float targetSpeed) {
        if (Player.Body.velocityY < StateMachine.MovementData.jumpHangTimeThreshold) {
            targetSpeed *= StateMachine.MovementData.jumpHangMaxSpeedMult;

        }
        return targetSpeed;
    }


    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
