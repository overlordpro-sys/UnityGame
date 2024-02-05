using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Code.Scripts.Player {
    public class PlayerNetwork : NetworkBehaviour {
        [SerializeField] private Rigidbody2D _body;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private InputActionReference _moveAction;
        [SerializeField] private InputActionReference _jumpAction;
        private Vector2 _moveDirection;


        [SerializeField] private float _acceleration;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _jumpForce;

        [SerializeField] private float _groundBoxY;
        [SerializeField] private LayerMask _groundLayer;


        public override void OnNetworkSpawn() {
            if (IsOwner) {
            }
        }

        public bool IsGrounded() {
            if (!IsOwner) {
                return false;
            }
            return Physics2D.BoxCast(_body.position, new Vector2(_collider.bounds.extents.x * 2, _groundBoxY), 0, Vector2.down, _collider.bounds.extents.y, _groundLayer);
        }


        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(_body.position + Vector2.down * _collider.bounds.extents.y, new Vector2(_collider.bounds.extents.x * 2, _groundBoxY));
        }

        private void FixedUpdate() {
            bool grounded = IsGrounded();
            if (!IsOwner) {
                return;
            }
            // accelerate player in direction of left or right input until max speed
            if (_moveDirection.x != 0) {
                // halve speed if not grounded
                if (!grounded) {
                    _moveDirection.x /= 2;
                }
                _body.velocity = new Vector2(Mathf.MoveTowards(_body.velocity.x, _maxSpeed * _moveDirection.x, _acceleration * Time.fixedDeltaTime), _body.velocity.y);
            }
            else if (grounded) {
                _body.velocity = new Vector2(Mathf.MoveTowards(_body.velocity.x, 0, _acceleration * Time.fixedDeltaTime), _body.velocity.y);
            }

            if (grounded && _jumpAction.action.IsPressed()) {
                Debug.Log("Jumped");
                _body.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            }
        }

        // Update is called once per frame
        private void Update() {
            if (!IsOwner) {
                return;
            }
            _moveDirection = _moveAction.action.ReadValue<Vector2>();
        }
    }
}

