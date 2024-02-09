using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Code.Scripts.Player {
    public class PlayerColliderScript : NetworkBehaviour {
        [SerializeField] private PlayerScript PlayerScript;

        // Ground
        [SerializeField] private float _groundBoxXOffset; // Offset from player collider box width
        [SerializeField] private float _groundBoxY;
        [SerializeField] private LayerMask _groundLayer;
        public override void OnNetworkSpawn() {

        }

        // Update is called once per frame
        void Update() {

        }

        internal bool IsGrounded() {
            return Physics2D.BoxCast(PlayerScript.Body.position, new Vector2(PlayerScript.Collider.bounds.extents.x * 2 + _groundBoxXOffset, _groundBoxY), 0, Vector2.down, PlayerScript.Collider.bounds.extents.y, _groundLayer);
        }
    }
}