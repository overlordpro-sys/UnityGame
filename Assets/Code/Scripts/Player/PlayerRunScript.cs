using System.Collections;
using System.Collections.Generic;
using Assets.Code.Scripts.Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code.Scripts.Player {
    [RequireComponent(typeof(PlayerScript))]
    public class PlayerRunScript : NetworkBehaviour {
        [SerializeField] private PlayerScript PlayerScript;
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
            PlayerScript.LastOnGroundTime -= Time.deltaTime;

            Vector2 moveDirection = PlayerScript.InputScript.MoveDirection;
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

        private void Run() {
            float targetSpeed = PlayerScript.InputScript.MoveDirection.x * _data.runMaxSpeed;
            targetSpeed = Mathf.Lerp(PlayerScript.Body.velocityX, targetSpeed, 1);
            float accelRate;

            // Different accelerations based on grounded
            if (PlayerScript.LastOnGroundTime > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _data.runAccelAmount : _data.runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _data.runAccelAmount * _data.accelInAir : _data.runDeccelAmount * _data.deccelInAir;


            //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
            if ((PlayerScript.JumpScript._isJumping || PlayerScript.JumpScript._isJumpFalling) && Mathf.Abs(PlayerScript.Body.velocityY) < _data.jumpHangTimeThreshold) {
                accelRate *= _data.jumpHangAccelerationMult;
                targetSpeed *= _data.jumpHangMaxSpeedMult;
            }


            //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
            if (_data.doConserveMomentum && Mathf.Abs(PlayerScript.Body.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(PlayerScript.Body.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && PlayerScript.LastOnGroundTime < 0) {
                // Conserve are current momentum
                // You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
                accelRate = 0;
            }

            // Proportionally increase speed
            float speedDif = targetSpeed - PlayerScript.Body.velocityX;
            float movement = speedDif * accelRate;

            // Apply to rigid body
            PlayerScript.Body.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }



        private void CheckDirectionToFace(bool isMovingRight) {
            if (isMovingRight != _isFacingRight)
                Turn();
        }

        private void Turn() {
            //stores scale and flips the player along the x axis, 
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            _isFacingRight = !_isFacingRight;
        }
    }
}