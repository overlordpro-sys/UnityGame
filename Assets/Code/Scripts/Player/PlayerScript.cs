using System.Collections;
using System.Collections.Generic;
using Assets.Code.Scripts.Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Code.Scripts.Player {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(PlayerRunScript))]
    [RequireComponent(typeof(PlayerJumpScript))]
    [RequireComponent(typeof(PlayerInputScript))]
    [RequireComponent(typeof(PlayerColliderScript))]
    public class PlayerScript : NetworkBehaviour {
        // Physics objects
        [SerializeField] internal Rigidbody2D Body;
        [SerializeField] internal Collider2D Collider;

        // Child Scripts
        [SerializeField] internal PlayerRunScript RunScript;
        [SerializeField] internal PlayerJumpScript JumpScript;
        [SerializeField] internal PlayerInputScript InputScript;
        [SerializeField] internal PlayerColliderScript ColliderScript;

        // Timers
        internal float LastOnGroundTime;



        public override void OnNetworkSpawn() {

        }

        internal void SetGravityScale(float scale) {
            Body.gravityScale = scale;
        }


        // Update is called once per frame
        void Update() {

        }
    }
}