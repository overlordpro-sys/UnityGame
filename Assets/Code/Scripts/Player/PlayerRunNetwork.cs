using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Code.Scripts.Player {
    public class PlayerNetwork : NetworkBehaviour {
        [SerializeField] private PlayerRunData Data;

        // Physics objects
        [SerializeField] private Rigidbody2D _body;
        [SerializeField] private Collider2D _collider;

        // State check
        private bool _isFacingRight;
        private float _lastOnGroundTime;
        [SerializeField] private float _groundBoxXOffset; // Offset from player collider box width
        [SerializeField] private float _groundBoxY;
        [SerializeField] private LayerMask _groundLayer;

        // Input
        private Vector2 _moveDirection;
        [SerializeField] private InputActionReference _moveAction;
        [SerializeField] private InputActionReference _jumpAction;

        public override void OnNetworkSpawn() {
            if (IsOwner) {
                _isFacingRight = true;
            }
        }

        private void FixedUpdate() {
            bool grounded = IsGrounded();
            if (!IsOwner) {
                return;
            }

            Run();
            //if (grounded && _jumpAction.action.IsPressed()) {
            //    Debug.Log("Jumped");
            //    _body.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            //}
        }

        // Update is called once per frame
        private void Update() {
            if (!IsOwner) {
                return;
            }
            _lastOnGroundTime -= Time.deltaTime;

            // Get x movement
            _moveDirection = _moveAction.action.ReadValue<Vector2>();

            if (_moveDirection.x != 0)
                CheckDirectionToFace(_moveDirection.x > 0);

            if (IsGrounded()) //checks if set box overlaps with ground
                _lastOnGroundTime = 0.1f;

        }

        // Run
        private void Run() {
            float targetSpeed = _moveDirection.x * Data.runMaxSpeed;

            float accelRate;

            // Different accelerations based on grounded
            if (_lastOnGroundTime > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;

            /* 
            //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
            if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(_body.velocity.y) < Data.jumpHangTimeThreshold)
            {
                accelRate *= Data.jumpHangAccelerationMult;
                targetSpeed *= Data.jumpHangMaxSpeedMult;
            }
            */

            //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
            if (Data.doConserveMomentum && Mathf.Abs(_body.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(_body.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && _lastOnGroundTime < 0) {
                // Conserve are current momentum
                // You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
                accelRate = 0;
            }

            // Proportionally increase speed
            float speedDif = targetSpeed - _body.velocity.x;
            float movement = speedDif * accelRate;

            // Apply to rigid body
            _body.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }

        private void Turn() {
            //stores scale and flips the player along the x axis, 
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            _isFacingRight = !_isFacingRight;
        }


        // Checks
        public bool IsGrounded() {
            if (!IsOwner) {
                return false;
            }
            return Physics2D.BoxCast(_body.position, new Vector2(_collider.bounds.extents.x * 2 + _groundBoxXOffset, _groundBoxY), 0, Vector2.down, _collider.bounds.extents.y, _groundLayer);
        }

        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(_body.position + Vector2.down * _collider.bounds.extents.y, new Vector2(_collider.bounds.extents.x * 2 + _groundBoxXOffset, _groundBoxY));
        }

        private void CheckDirectionToFace(bool isMovingRight) {
            if (isMovingRight != _isFacingRight)
                Turn();
        }
    }
}

