using System;
using UnityEngine;

public class PlayerState {
    protected Player Player;
    protected PlayerStateMachine StateMachine;

    public PlayerState(Player player, PlayerStateMachine stateMachine) {
        this.Player = player;
        this.StateMachine = stateMachine;

    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }

    public virtual void FrameUpdate() {

    }

    public virtual void PhysicsUpdate() {
        Vector3 scale = Player.transform.localScale;
        // check if scale.x and MoveDirection.x don't have the same sign
        if (scale.x * Player.InputScript.MoveDirection.x < 0) {
            scale.x *= -1;
            Player.transform.localScale = scale;
        }

        float targetSpeed = Player.InputScript.MoveDirection.x * Player.MovementData.runMaxSpeed;
        targetSpeed = Mathf.Lerp(Player.Body.velocityX, targetSpeed, 1);

        float accelRate;
        // Different accelerations based on grounded
        if (Player.TimerManager.LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Player.MovementData.runAccelAmount : Player.MovementData.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Player.MovementData.runAccelAmount * Player.MovementData.accelInAir : Player.MovementData.runDeccelAmount * Player.MovementData.deccelInAir;

        //We won't slow the Player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        //if (_data.doConserveMomentum && Mathf.Abs(_player.Body.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(_player.Body.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && _player.TimerManager.LastOnGroundTime < 0) {
        //    // Conserve are current momentum
        //    accelRate = 0;
        //}


        // Base modifications
        accelRate = ModifyAccel(accelRate);
        targetSpeed = ModifyTargetSpeed(targetSpeed);

        // Proportionally increase speed
        float speedDif = targetSpeed - Player.Body.velocityX;
        float movement = speedDif * accelRate;

        // Apply to rigid body
        Player.Body.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }

    protected virtual float ModifyAccel(float accelRate) {
        return accelRate;
    }

    protected virtual float ModifyTargetSpeed(float targetSpeed) {
        return targetSpeed;
    }

    protected Boolean CanJump() {
        return Player.TimerManager.LastOnGroundTime > 0;
    }

    protected Boolean IsJumpHang() {
        return Mathf.Abs(Player.Body.velocityY) < Player.MovementData.jumpHangTimeThreshold;
    }


    public virtual void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) { }
}