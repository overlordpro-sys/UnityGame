using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Player))]
public class PlayerRunScript : NetworkBehaviour {
    [SerializeField] private Player _player;
    [SerializeField] private PlayerMovementData _data;

    // Run
    private bool _isFacingRight;


    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            return;
        }
        _isFacingRight = true;
    }


    // Update is called once per frame
    void Update() {
        if (!IsOwner) {
            return;
        }

        // Timers
        _player.TimerManager.LastOnGroundTime -= Time.deltaTime;

        Vector2 moveDirection = _player.InputScript.MoveDirection;
        if (moveDirection.x != 0) {
            CheckDirectionToFace(moveDirection.x > 0);
        }
    }

    private void FixedUpdate() {
        if (!IsOwner) {
            return;
        }

        Run();
    }

    internal void Run() {
        float targetSpeed = _player.InputScript.MoveDirection.x * _data.runMaxSpeed;
        targetSpeed = Mathf.Lerp(_player.Body.velocityX, targetSpeed, 1);
        float accelRate;

        // Different accelerations based on grounded
        if (_player.TimerManager.LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _data.runAccelAmount : _data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _data.runAccelAmount * _data.accelInAir : _data.runDeccelAmount * _data.deccelInAir;


        //Increase our acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((_player.JumpScript._isJumping || _player.JumpScript._isJumpFalling) && Mathf.Abs(_player.Body.velocityY) < _data.jumpHangTimeThreshold) {
            accelRate *= _data.jumpHangAccelerationMult;
            targetSpeed *= _data.jumpHangMaxSpeedMult;
        }


        //We won't slow the Player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (_data.doConserveMomentum && Mathf.Abs(_player.Body.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(_player.Body.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && _player.TimerManager.LastOnGroundTime < 0) {
            // Conserve are current momentum
            accelRate = 0;
        }

        // Proportionally increase speed
        float speedDif = targetSpeed - _player.Body.velocityX;
        float movement = speedDif * accelRate;

        // Apply to rigid body
        _player.Body.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }



    private void CheckDirectionToFace(bool isMovingRight) {
        if (isMovingRight != _isFacingRight)
            Turn();
    }

    private void Turn() {
        //stores scale and flips the Player along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        _isFacingRight = !_isFacingRight;
    }
}
