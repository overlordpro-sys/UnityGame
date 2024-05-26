using System.Collections;
using System.Collections.Generic;
using Assets.InputSystem;
using Assets.Scripts.GameState.Classes;
using Assets.Scripts.Players;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Players {
    public class Player : MonoBehaviour {
        [Header("GameObjects")]
        [SerializeField] internal GameObject Body;
        [SerializeField] internal GameObject Turret;

        [Header("Components")]
        [SerializeField] internal Rigidbody2D Rigidbody;
        [SerializeField] internal Collider2D Collider;
        [SerializeField] internal PlayerInput PlayerInput;
        internal PlayerControls PlayerInputActions;

        [Header("Scripts")]
        [SerializeField] internal PlayerHealth PlayerHealth;
        [SerializeField] internal PlayerMovement PlayerMovement;
        [SerializeField] internal PlayerBoost PlayerBoost;
        [SerializeField] internal PlayerTurret PlayerTurret;

        internal PlayerConfig PlayerConfig;

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