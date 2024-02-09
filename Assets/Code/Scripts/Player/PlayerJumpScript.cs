using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Code.Scripts.Player {
    [RequireComponent(typeof(PlayerScript))]
    [RequireComponent(typeof(PlayerMovementData))]
    public class PlayerJumpScript : NetworkBehaviour {
        [SerializeField] private PlayerScript PlayerScript;
        [SerializeField] private PlayerMovementData _data;

        // Jump
        private float _lastPressedJumpTime; //  for input buffer
        internal bool _isJumping;
        internal bool _isJumpFalling;
        private bool _isJumpCut;

        public override void OnNetworkSpawn() {
            if (!IsOwner) {
                return;
            }

            PlayerScript.InputScript.JumpAction.action.started += OnJumpDown;
            PlayerScript.InputScript.JumpAction.action.canceled += OnJumpUp;
        }

        // Update is called once per frame
        void Update() {
            if (!IsOwner) {
                return;
            }

            // Time since was last grounded for leniency
            if (!_isJumping && PlayerScript.ColliderScript.IsGrounded()) { //checks if set box overlaps with ground
                PlayerScript.LastOnGroundTime = _data.coyoteTime;
            }

            // Jump checks
            if (_isJumping && PlayerScript.Body.velocity.y < 0) { // If falling, no longer jumping
                _isJumping = false;
            }

            if (PlayerScript.LastOnGroundTime > 0 && !_isJumping) { // If grounded or just grounded, reset jump cut and falling
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
            if (PlayerScript.Body.velocityY < 0 && PlayerScript.InputScript.MoveDirection.y < 0) { // if falling and pressing down
                PlayerScript.SetGravityScale(_data.gravityScale * _data.fastFallGravityMult);
                PlayerScript.Body.velocity = new Vector2(PlayerScript.Body.velocity.x, Mathf.Min(PlayerScript.Body.velocity.y, -_data.maxFastFallSpeed));
            }
            else if (_isJumpCut) {
                PlayerScript.SetGravityScale(_data.gravityScale * _data.jumpCutGravityMult);
                PlayerScript.Body.velocity = new Vector2(PlayerScript.Body.velocity.x, Mathf.Min(PlayerScript.Body.velocity.y, -_data.maxFallSpeed));
            }
            else if ((_isJumping || _isJumpFalling) && Mathf.Abs(PlayerScript.Body.velocityY) < _data.jumpHangTimeThreshold) {
                PlayerScript.SetGravityScale(_data.gravityScale * _data.jumpHangGravityMult);
            }
            else if (PlayerScript.Body.velocity.y < 0) {
                //Higher gravity if falling
                PlayerScript.SetGravityScale(_data.gravityScale * _data.fallGravityMult);
                PlayerScript.Body.velocity = new Vector2(PlayerScript.Body.velocity.x, Mathf.Min(PlayerScript.Body.velocity.y, -_data.maxFallSpeed));
            }
            else {
                //Default gravity if standing on a platform or moving upwards
                PlayerScript.SetGravityScale(_data.gravityScale);
            }
        }

        // Jump
        private void OnJumpDown(InputAction.CallbackContext context) {
            _lastPressedJumpTime = _data.jumpInputBufferTime;
        }

        private void OnJumpUp(InputAction.CallbackContext context) {
            if (CanJumpCut()) {
                _isJumpCut = true;
            }
        }
        private void Jump() {
            _lastPressedJumpTime = 0;
            PlayerScript.LastOnGroundTime = 0;
            PlayerScript.Body.velocityY = 0;

            PlayerScript.Body.AddForceY(_data.jumpForce, ForceMode2D.Impulse);
        }

        // Checks

        public bool CanJump() {
            return PlayerScript.LastOnGroundTime > 0 && !_isJumping; // if was grounded within coyote time
        }

        public bool CanJumpCut() {
            return _isJumping && PlayerScript.Body.velocityY > 0; // if jumping on the way up
        }
    }
}