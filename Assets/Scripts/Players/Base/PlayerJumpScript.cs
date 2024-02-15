using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputScript))]
public class PlayerJumpScript : NetworkBehaviour {
    [SerializeField] private Player _player;
    [SerializeField] private PlayerMovementData _data;

    // Jump
    internal bool _isJumping;
    internal bool _isJumpFalling;
    private bool _isJumpCut;

    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            return;
        }

        _player.InputScript.JumpAction.action.started += OnJumpDown;
        _player.InputScript.JumpAction.action.canceled += OnJumpUp;
    }

    // Update is called once per frame
    void Update() {
        if (!IsOwner) {
            return;
        }

        // Time since was last grounded for leniency
        if (!_isJumping && _player.ColliderScript.IsGrounded()) { //checks if set box overlaps with ground
            _player.TimerManager.LastOnGroundTime = _data.coyoteTime;
        }

        // Jump checks
        if (_isJumping && _player.Body.velocity.y < 0) { // If falling, no longer jumping
            _isJumping = false;
        }

        if (_player.TimerManager.LastOnGroundTime > 0 && !_isJumping) { // If grounded or just grounded, reset jump cut and falling
            _isJumpCut = false;
            _isJumpFalling = false;
        }

        // Jump
        if (CanJump() && _player.TimerManager.LastPressedJumpTime > 0) { // if allowed to jump and within jump input buffer time
            _isJumping = true;
            _isJumpCut = false;
            _isJumpFalling = false;
            Jump();
        }

        // Gravity
        if (_player.Body.velocityY < 0 && _player.InputScript.MoveDirection.y < 0) { // if falling and pressing down
            _player.SetGravityScale(_data.gravityScale * _data.fastFallGravityMult);
            _player.Body.velocity = new Vector2(_player.Body.velocity.x, Mathf.Min(_player.Body.velocity.y, -_data.maxFastFallSpeed));
        }
        else if (_isJumpCut) {
            _player.SetGravityScale(_data.gravityScale * _data.jumpCutGravityMult);
            _player.Body.velocity = new Vector2(_player.Body.velocity.x, Mathf.Min(_player.Body.velocity.y, -_data.maxFallSpeed));
        }
        else if ((_isJumping || _isJumpFalling) && Mathf.Abs(_player.Body.velocityY) < _data.jumpHangTimeThreshold) {
            _player.SetGravityScale(_data.gravityScale * _data.jumpHangGravityMult);
        }
        else if (_player.Body.velocity.y < 0) {
            //Higher gravity if falling
            _player.SetGravityScale(_data.gravityScale * _data.fallGravityMult);
            _player.Body.velocity = new Vector2(_player.Body.velocity.x, Mathf.Min(_player.Body.velocity.y, -_data.maxFallSpeed));
        }
        else {
            //Default gravity if standing on a platform or moving upwards
            _player.SetGravityScale(_data.gravityScale);
        }
    }

    // Jump
    private void OnJumpDown(InputAction.CallbackContext context) {
        _player.TimerManager.LastPressedJumpTime = _data.jumpInputBufferTime;
    }

    private void OnJumpUp(InputAction.CallbackContext context) {
        if (CanJumpCut()) {
            _isJumpCut = true;
        }
    }
    private void Jump() {
        _player.TimerManager.LastPressedJumpTime = 0;
        _player.TimerManager.LastOnGroundTime = 0;
        _player.Body.velocityY = 0;

        _player.Body.AddForceY(_data.jumpForce, ForceMode2D.Impulse);
    }

    // Checks

    public bool CanJump() {
        return _player.TimerManager.LastOnGroundTime > 0 && !_isJumping; // if was grounded within coyote time
    }

    public bool CanJumpCut() {
        return _isJumping && _player.Body.velocityY > 0; // if jumping on the way up
    }
}