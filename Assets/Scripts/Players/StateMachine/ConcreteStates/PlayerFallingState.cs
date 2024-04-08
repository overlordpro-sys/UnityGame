using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerState {
    public PlayerFallingState(PlayerBase playerBase, PlayerStateMachine stateMachine) : base(playerBase, stateMachine) {
    }

    public override void EnterState() {
        PlayerBase.SetGravityScale(PlayerBase.MovementData.gravityScale * PlayerBase.MovementData.fallGravityMult);
        PlayerBase.AnimationManager.SetAnimation(PlayerAnimationType.Fall);
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
        if (PlayerBase.ColliderScript.IsGrounded()) {
            if (Mathf.Abs(PlayerBase.Body.velocityX) <= PlayerBase.MovementData.stillSpeedThreshold) {
                StateMachine.ChangeState(PlayerBase.IdleState);
            }
            else {
                StateMachine.ChangeState(PlayerBase.RunState);
            }
        }
    }

    protected override float ModifyAccel(float accelRate) {
        //Increase our acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if (Mathf.Abs(PlayerBase.Body.velocityY) < PlayerBase.MovementData.jumpHangTimeThreshold) {
            accelRate *= PlayerBase.MovementData.jumpHangAccelerationMult;
        }
        return accelRate;
    }

    protected override float ModifyTargetSpeed(float targetSpeed) {
        if (Mathf.Abs(PlayerBase.Body.velocityY) < PlayerBase.MovementData.jumpHangTimeThreshold) {
            targetSpeed *= PlayerBase.MovementData.jumpHangMaxSpeedMult;
        }
        return targetSpeed;
    }

    public override void AnimationTriggerEvent(PlayerBase.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
