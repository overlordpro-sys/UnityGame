using System;
using UnityEngine;

public class PlayerState {
    protected PlayerBase PlayerBase;
    protected PlayerStateMachine StateMachine;

    public PlayerState(PlayerBase playerBase, PlayerStateMachine stateMachine) {
        this.PlayerBase = playerBase;
        this.StateMachine = stateMachine;

    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }

    public virtual void FrameUpdate() {

    }

    public virtual void PhysicsUpdate() {
        Vector3 scale = PlayerBase.transform.localScale;
        // check if scale.x and MoveDirection.x don't have the same sign
        if (scale.x * PlayerBase.InputScript.MoveDirection.x > 0) {
            scale.x *= -1;
            PlayerBase.transform.localScale = scale;
        }

        float targetSpeed = PlayerBase.InputScript.MoveDirection.x * PlayerBase.MovementData.runMaxSpeed;
        targetSpeed = Mathf.Lerp(PlayerBase.Body.velocityX, targetSpeed, 1);

        float accelRate;
        // Different accelerations based on grounded
        if (PlayerBase.TimerManager.LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? PlayerBase.MovementData.runAccelAmount : PlayerBase.MovementData.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? PlayerBase.MovementData.runAccelAmount * PlayerBase.MovementData.accelInAir : PlayerBase.MovementData.runDeccelAmount * PlayerBase.MovementData.deccelInAir;

        //We won't slow the PlayerBase down if they are moving in their desired direction but at a greater speed than their maxSpeed
        //if (_data.doConserveMomentum && Mathf.Abs(_playerBase.Body.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(_playerBase.Body.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && _playerBase.TimerManager.LastOnGroundTime < 0) {
        //    // Conserve are current momentum
        //    accelRate = 0;
        //}


        // Base modifications
        accelRate = ModifyAccel(accelRate);
        targetSpeed = ModifyTargetSpeed(targetSpeed);

        // Proportionally increase speed
        float speedDif = targetSpeed - PlayerBase.Body.velocityX;
        float movement = speedDif * accelRate;

        // Apply to rigid body
        PlayerBase.Body.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }

    protected virtual float ModifyAccel(float accelRate) {
        return accelRate;
    }

    protected virtual float ModifyTargetSpeed(float targetSpeed) {
        return targetSpeed;
    }

    protected Boolean CanJump() {
        return PlayerBase.TimerManager.LastOnGroundTime > 0;
    }

    protected Boolean IsJumpHang() {
        return Mathf.Abs(PlayerBase.Body.velocityY) < PlayerBase.MovementData.jumpHangTimeThreshold;
    }


    public virtual void AnimationTriggerEvent(PlayerBase.AnimationTriggerType triggerType) { }
}
