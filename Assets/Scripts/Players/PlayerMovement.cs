using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Assets.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;

namespace Assets.Scripts.Players {
    public class PlayerMovement : MonoBehaviour, IMoveable {
        [SerializeField] private Player player;

        private Vector2 moveInputVector;
        private bool movePressed = false;

        [SerializeField] private float thrustPower = 50f;
        [SerializeField] private float rotationPower = 2f;

        void Start() {
            if (player == null) {
                Debug.LogError("Player missing reference", this);
            }

            moveInputVector = Vector2.zero;

            player.PlayerInputActions.Player.Move.performed += (context => movePressed = true);
            player.PlayerInputActions.Player.Move.canceled += (context => movePressed = false);
        }

        private void ApplyThrust() {
            float thrustMult = Vector2.Dot(transform.right, moveInputVector);
            if (thrustMult > 0) {
                player.Rigidbody.AddForce(transform.right * thrustMult * thrustPower);
            }
        }

        private void ApplyRotation() {
            float targetAngle = Mathf.Atan2(moveInputVector.y, moveInputVector.x) * Mathf.Rad2Deg;
            player.Rigidbody.rotation = Mathf.LerpAngle(player.Rigidbody.rotation, targetAngle, rotationPower * Time.fixedDeltaTime);
        }

        private void Update() {
            moveInputVector = player.PlayerInputActions.Player.Move.ReadValue<Vector2>().normalized;
        }

        private void FixedUpdate() {
            if (movePressed) {
                ApplyThrust();
                ApplyRotation();
            }
        }

        public void TakeKnockback(float knockbackForce, Transform other) {
            player.Rigidbody.AddForce(knockbackForce * (other.position - player.transform.position).normalized, ForceMode2D.Impulse);
        }
    }
}