using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using IngameDebugConsole;
using Unity.Netcode;
using UnityEngine;

public struct PlayerAnimationType {
    public const string Idle = "Base Layer.Idle";
    public const string Run = "Base Layer.Run";
    public const string Jump = "Base Layer.Jump";
    public const string Climb = "Base Layer.Climb";
    public const string Fall = "Base Layer.Jump";
}


public class PlayerAnimationManager : NetworkBehaviour {
    private Animator Animator { get; set; }

    private void Awake() {
        Animator = GetComponent<Animator>();
    }

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            DebugLogConsole.AddCommandInstance("playerBase.setvariant", "Set the playerBase's animation character", "SetAnimationControllerServerRpc", this);
        }
    }

    public void SetAnimationOverrideControllerFromResources(PlayerCharacter character) {
        SetAnimationControllerServerRpc(character.ResourcePath);
    }

    [Rpc(SendTo.Server)]
    private void SetAnimationControllerServerRpc(string path) {
        SetAnimationControllerClientRpc(path);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SetAnimationControllerClientRpc(string path) {
        Animator.runtimeAnimatorController = Resources.Load(path) as RuntimeAnimatorController;
    }



    public void SetAnimation(string animationName) {
        AnimationServerRpc(NetworkObjectId, animationName);
    }

    [Rpc(SendTo.Server)]
    private void AnimationServerRpc(ulong sourceNetworkObjectId, string animationName) {
        AnimationClientRpc(sourceNetworkObjectId, animationName);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void AnimationClientRpc(ulong sourceNetworkObjectId, string animationName) {
        Animator.Play(animationName);
    }

    // Update is called once per frame
    private void Update() {
        if (!IsOwner) {
            return;
        }

    }
}


