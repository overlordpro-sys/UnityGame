using Assets.InputSystem;
using Assets.Scripts.GameState.Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Ship {
    public class Ship : MonoBehaviour {
        [Header("GameObjects")]
        [SerializeField] internal GameObject Body;
        [SerializeField] internal GameObject Turret;

        [Header("Components")]
        [SerializeField] internal Rigidbody2D Rigidbody;
        [SerializeField] internal Collider2D Collider;
        [SerializeField] internal PlayerInput PlayerInput;
        internal PlayerControls PlayerInputActions;

        [Header("Scripts")]
        [SerializeField] internal ShipHealth PlayerHealth;
        [SerializeField] internal ShipMovement PlayerMovement;
        [SerializeField] internal ShipBoost PlayerBoost;
        [SerializeField] internal ShipTurret PlayerTurret;

        internal PlayerData PlayerData;

        public void Start() {
            if (PlayerHealth == null) {
                Debug.LogError("PlayerHealth missing reference", this);
            }

            if (PlayerMovement == null) {
                Debug.LogError("PlayerMovement missing reference", this);
            }

            if (PlayerBoost == null) {
                Debug.LogError("PlayerBoost missing reference", this);
            }

            if (PlayerTurret == null) {
                Debug.LogError("PlayerTurret missing reference", this);
            }
        }

        public void Awake() {
            PlayerInputActions = new PlayerControls();
            PlayerInputActions.Enable();
        }
    }
}