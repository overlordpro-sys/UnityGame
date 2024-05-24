using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Assets.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;

namespace Assets.Scripts.Players {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour {
        private new Rigidbody2D rigidbody;
        private PlayerControls playerInputActions;
        private Vector2 inputVector;
        private bool movePressed = false;

        public float thrustPower = 50f;
        public float rotationPower = 2f;
        void Start() {
            rigidbody = GetComponent<Rigidbody2D>();
            inputVector = Vector2.zero;

            playerInputActions = new PlayerControls();
            playerInputActions.Player.Move.performed += MovePressed;
            playerInputActions.Player.Move.canceled += MoveReleased;
            playerInputActions.Enable();

        }

        private void ApplyThrust() {
            float thrustMult = Vector2.Dot(transform.right, inputVector);
            if (thrustMult > 0) {
                rigidbody.AddForce(transform.right * thrustMult * thrustPower);
            }
        }

        private void ApplyRotation() {
            float targetAngle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;
            rigidbody.rotation = Mathf.LerpAngle(rigidbody.rotation, targetAngle, rotationPower * Time.fixedDeltaTime);
        }

        private void MoveReleased(InputAction.CallbackContext ctx) {
            movePressed = false;
        }

        private void MovePressed(InputAction.CallbackContext ctx) {
            movePressed = true;
        }

        private void Update() {
            inputVector = playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
        }

        private void FixedUpdate() {
            if (movePressed) {
                ApplyThrust();
                ApplyRotation();
            }
        }

    }
}