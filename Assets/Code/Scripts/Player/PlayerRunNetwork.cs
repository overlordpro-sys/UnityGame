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

        // Input
        private Vector2 _moveDirection;
        [SerializeField] private InputActionReference _moveAction;
        [SerializeField] private InputActionReference _jumpAction;

        // Run
        private float _lastOnGroundTime;
        private bool _isFacingRight;

        // Ground
        [SerializeField] private float _groundBoxXOffset; // Offset from player collider box width
        [SerializeField] private float _groundBoxY;
        [SerializeField] private LayerMask _groundLayer;

        // Jump
        private float _lastPressedJumpTime; //  for input buffer
        private bool _isJumping;
        private bool _isJumpFalling;
        private bool _isJumpCut;

        public override void OnNetworkSpawn() {
            if (!IsOwner) {
                return;
            }
            _jumpAction.action.Enable();
            _jumpAction.action.started += OnJumpDown;
            _jumpAction.action.canceled += OnJumpUp;
            _isFacingRight = true;
        }

        // Update is called once per frame
        private void Update() {
            if (!IsOwner) {
                return;
            }

            // Timers
            _lastOnGroundTime -= Time.deltaTime;
            _lastPressedJumpTime -= Time.deltaTime;

            // Input
            _moveDirection = _moveAction.action.ReadValue<Vector2>(); // Get x movement
            if (_moveDirection.x != 0)
                CheckDirectionToFace(_moveDirection.x > 0);

            // Time since was last grounded for leniency
            if (!_isJumping && IsGrounded()) { //checks if set box overlaps with ground
                _lastOnGroundTime = Data.coyoteTime;
            }

            // Jump checks
            if (_isJumping && _body.velocity.y < 0) { // If falling, no longer jumping
                _isJumping = false;
            }

            if (_lastOnGroundTime > 0 && !_isJumping) { // If grounded or just grounded, reset jump cut and falling
                _isJumpCut = false;
                _isJumpFalling = false;
            }

            // Jump
            if (CanJump() && _lastPressedJumpTime > 0) { // if allowed to jump and within jump input buffer time
                _isJumping = true;
                _isJumpCut = false;
                _isJumpFalling = false;
                Jump();
            }

            // Gravity
            if (_body.velocityY < 0 && _moveDirection.y < 0) { // if falling and pressing down
                SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
                _body.velocity = new Vector2(_body.velocity.x, Mathf.Max(_body.velocity.y, -Data.maxFastFallSpeed));
            }
            else if (_isJumpCut) {
                SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
                _body.velocity = new Vector2(_body.velocity.x, Mathf.Max(_body.velocity.y, -Data.maxFallSpeed));
            }
            else if ((_isJumping || _isJumpFalling) && Mathf.Abs(_body.velocityY) < Data.jumpHangTimeThreshold) {
                SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
            }
            else if (_body.velocity.y < 0) {
                //Higher gravity if falling
                SetGravityScale(Data.gravityScale * Data.fallGravityMult);
                _body.velocity = new Vector2(_body.velocity.x, Mathf.Max(_body.velocity.y, -Data.maxFallSpeed));
            }
            else {
                //Default gravity if standing on a platform or moving upwards
                SetGravityScale(Data.gravityScale);
            }
        }
        private void FixedUpdate() {
            if (!IsOwner) {
                return;
            }

            Run();
        }

        // Run
        private void Run() {
            float targetSpeed = _moveDirection.x * Data.runMaxSpeed;
            targetSpeed = Mathf.Lerp(_body.velocityX, targetSpeed, 1);
            float accelRate;

            // Different accelerations based on grounded
            if (_lastOnGroundTime > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;


            //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
            if ((_isJumping || _isJumpFalling) && Mathf.Abs(_body.velocityY) < Data.jumpHangTimeThreshold) {
                accelRate *= Data.jumpHangAccelerationMult;
                targetSpeed *= Data.jumpHangMaxSpeedMult;
            }


            //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
            if (Data.doConserveMomentum && Mathf.Abs(_body.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(_body.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && _lastOnGroundTime < 0) {
                // Conserve are current momentum
                // You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
                accelRate = 0;
            }

            // Proportionally increase speed
            float speedDif = targetSpeed - _body.velocityX;
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

        // Jump
        private void OnJumpDown(InputAction.CallbackContext context) {
            _lastPressedJumpTime = Data.jumpInputBufferTime;
        }

        private void OnJumpUp(InputAction.CallbackContext context) {
            if (CanJumpCut()) {
                _isJumpCut = true;
            }
        }
        private void Jump() {
            _lastPressedJumpTime = 0;
            _lastOnGroundTime = 0;
            _body.velocityY = 0;

            _body.AddForceY(Data.jumpForce, ForceMode2D.Impulse);
        }

        private void SetGravityScale(float scale) {
            _body.gravityScale = scale;
        }

        // Checks
        public bool IsGrounded() {
            if (!IsOwner) {
                return false;
            }
            return Physics2D.BoxCast(_body.position, new Vector2(_collider.bounds.extents.x * 2 + _groundBoxXOffset, _groundBoxY), 0, Vector2.down, _collider.bounds.extents.y, _groundLayer);
        }

        public bool CanJump() {
            return _lastOnGroundTime > 0 && !_isJumping; // if was grounded within coyote time
        }

        public bool CanJumpCut() {
            return _isJumping && _body.velocityY > 0; // if jumping on the way up
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

