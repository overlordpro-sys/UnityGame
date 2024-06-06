using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Assets.Scripts.Ship {
    public class ShipMovement : MonoBehaviour, IMoveable {
        [SerializeField] private Ship ship;

        private Vector2 moveInputVector;
        private bool movePressed = false;

        [SerializeField] private float thrustPower = 50f;
        [SerializeField] private float rotationPower = 2f;

        void Start() {
            if (ship == null) {
                Debug.LogError("Player missing reference", this);
            }

            moveInputVector = Vector2.zero;

            ship.PlayerInput.actions["Move"].performed += (context => movePressed = true);
            ship.PlayerInput.actions["Move"].canceled += (context => movePressed = false);
        }

        private void ApplyThrust() {
            float thrustMult = Vector2.Dot(transform.right, moveInputVector);
            if (thrustMult > 0) {
                ship.Rigidbody.AddForce(transform.right * thrustMult * thrustPower);
            }
        }

        private void ApplyRotation() {
            float targetAngle = Mathf.Atan2(moveInputVector.y, moveInputVector.x) * Mathf.Rad2Deg;
            ship.Rigidbody.rotation = Mathf.LerpAngle(ship.Rigidbody.rotation, targetAngle, rotationPower * Time.fixedDeltaTime);
        }

        private void Update() {
            moveInputVector = ship.PlayerInput.actions["Move"].ReadValue<Vector2>().normalized;
        }

        private void FixedUpdate() {
            if (movePressed) {
                ApplyThrust();
                ApplyRotation();
            }
        }

        public void TakeKnockback(float knockbackForce, Transform other) {
            ship.Rigidbody.AddForce(knockbackForce * (other.position - ship.transform.position).normalized, ForceMode2D.Impulse);
        }
    }
}