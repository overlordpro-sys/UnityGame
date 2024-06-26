using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts.Ship {
    [RequireComponent(typeof(Rigidbody2D))]
    public class ShipBoost : MonoBehaviour {
        [SerializeField] private Ship player;

        private bool boostPressed = false;
        private float timeSinceBoostReleased = 0;

        [SerializeField] private Image boostBar;

        public float BoostMaxCharge = 2; // seconds of boost available
        public float BoostCharge = 2; // 0 is 0 seconds remaining, 1 is 1 second remaining
        public float BoostRechargeRate = 0.25f; // seconds of boost to recharge per second
        public float BoostRechargeDelay = 2; // seconds to wait before recharging boost
        public float BoostPower = 100f;

        void Start() {

            player.PlayerInput.actions["Boost"].performed += BoostPressed;
            player.PlayerInput.actions["Boost"].canceled += BoostReleased;
        }


        private void ApplyBoost() {

            player.Rigidbody.AddForce(transform.right * BoostPower);

        }

        private void BoostPressed(InputAction.CallbackContext ctx) {
            boostPressed = true;
        }

        private void BoostReleased(InputAction.CallbackContext ctx) {
            boostPressed = false;
            timeSinceBoostReleased = 0;
        }

        private void FixedUpdate() {
            timeSinceBoostReleased += Time.fixedDeltaTime;
            // Apply boost
            if (boostPressed) {
                BoostCharge -= Time.fixedDeltaTime;
                timeSinceBoostReleased = 0;
                if (BoostCharge > 0) {
                    ApplyBoost();
                }
                else {
                    BoostCharge = 0;
                }
                UpdateBoostBar();
            }

            // Recharge boost
            if (timeSinceBoostReleased > BoostRechargeDelay && BoostCharge < BoostMaxCharge) {
                BoostCharge += BoostRechargeRate * Time.fixedDeltaTime;
                if (BoostCharge > BoostMaxCharge) {
                    BoostCharge = BoostMaxCharge;
                }
                UpdateBoostBar();
            }
        }

        private float GetPercentBoost() {
            return BoostCharge / BoostMaxCharge;
        }

        private void UpdateBoostBar() {
            boostBar.fillAmount = GetPercentBoost();
        }
    }
}