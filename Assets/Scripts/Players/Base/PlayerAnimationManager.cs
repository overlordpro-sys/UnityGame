using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct PlayerAnimationType {
    public const string Idle = "Base Layer.Idle";
    public const string Run = "Base Layer.Run";
    public const string Jump = "Base Layer.Jump";
    public const string Climb = "Base Layer.Climb";
    public const string Fall = "Base Layer.Fall";

}

public class PlayerAnimationManager : NetworkBehaviour {
    internal Animator Animator { get; set; }
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            return;
        }

        Animator = transform.Find("Sprite").GetComponent<Animator>();
    }

    public void SetAnimation(string animationName) {
        if (!IsOwner) {
            return;
        }
        Animator.Play(animationName);
    }

    // Update is called once per frame
    void Update() {
        if (!IsOwner) {
            return;
        }

    }
}
