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
        private Vector2 moveInputVector;
        private bool movePressed = false;

        [SerializeField] private float thrustPower = 50f;
        [SerializeField] private float rotationPower = 2f;

        void Start() {
            rigidbody = GetComponent<Rigidbody2D>();
            moveInputVector = Vector2.zero;

            playerInputActions = new PlayerControls();
            playerInputActions.Player.Move.performed += (context => movePressed = true);
            playerInputActions.Player.Move.canceled += (context => movePressed = false);
            playerInputActions.Enable();

        }

        private void ApplyThrust() {
            float thrustMult = Vector2.Dot(transform.right, moveInputVector);
            if (thrustMult > 0) {
                rigidbody.AddForce(transform.right * thrustMult * thrustPower);
            }
        }

        private void ApplyRotation() {
            float targetAngle = Mathf.Atan2(moveInputVector.y, moveInputVector.x) * Mathf.Rad2Deg;
            rigidbody.rotation = Mathf.LerpAngle(rigidbody.rotation, targetAngle, rotationPower * Time.fixedDeltaTime);
        }

        private void Update() {
            moveInputVector = playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
        }

        private void FixedUpdate() {
            if (movePressed) {
                ApplyThrust();
                ApplyRotation();
            }
        }

    }
}