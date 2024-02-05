using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code.Scripts.Player {
    public class PlayerNetwork : NetworkBehaviour {
        [SerializeField] private Rigidbody2D _body;

        private Vector2 _moveVector;
        [SerializeField] private float _acceleration = 5f;
        public override void OnNetworkSpawn() {
            //if (IsOwner) {

            //}
        }

        private void FixedUpdate() {
            if (!IsOwner) {
                return;
            }
            _body.AddForce(_moveVector * _acceleration);
        }


        // Update is called once per frame
        private void Update() {
            if (!IsOwner) {
                return;
            }
            _moveVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _moveVector.Normalize();
        }
    }
}

