using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputScript : NetworkBehaviour {
    [SerializeField] internal InputActionReference MoveAction;
    [SerializeField] internal InputActionReference JumpAction;

    // Input
    internal Vector2 MoveDirection;
    private void OnValidate() {
        if (MoveAction == null)
            Debug.LogWarning("MoveAction is not assigned in " + gameObject.name);

        if (JumpAction == null)
            Debug.LogWarning("JumpAction is not assigned in " + gameObject.name);
    }

    // Use this for initialization
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            return;
        }

        MoveAction.action.Enable();
        MoveAction.action.performed += ctx => MoveDirection = ctx.ReadValue<Vector2>();

        JumpAction.action.Enable();
    }

    // Update is called once per frame
    private void Update() {
        if (!IsOwner) {
            return;
        }
    }
}
