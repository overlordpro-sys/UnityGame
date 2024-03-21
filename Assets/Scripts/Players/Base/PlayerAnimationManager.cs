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

public class ResourcePathAttribute : Attribute {
    public string Path { get; private set; }

    public ResourcePathAttribute(string path) {
        this.Path = path;
    }

    public static string GetResourcePath(Enum Value) {
        Type Type = Value.GetType();

        FieldInfo FieldInfo = Type.GetField(Value.ToString());

        ResourcePathAttribute Attribute = FieldInfo.GetCustomAttribute(
            typeof(ResourcePathAttribute)
        ) as ResourcePathAttribute;

        return Attribute.Path;
    }
}

public enum PlayerVariant {
    [ResourcePath("Controllers/Variant A")]
    A,
    [ResourcePath("Controllers/Variant B")]
    B,
    [ResourcePath("Controllers/Variant C")]
    C,
    [ResourcePath("Controllers/Variant D")]
    D
}

public class PlayerAnimationManager : NetworkBehaviour {
    private Animator Animator { get; set; }

    void Awake() {
        Animator = GetComponent<Animator>();
    }

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            DebugLogConsole.AddCommandInstance("player.setvariant", "Set the player's animation variant", "SetAnimationControllerServerRpc", this);
        }
    }

    public void SetAnimationOverrideControllerFromResources(PlayerVariant variant) {
        SetAnimationControllerServerRpc(ResourcePathAttribute.GetResourcePath(variant));
    }

    [Rpc(SendTo.Server)]
    void SetAnimationControllerServerRpc(string path) {
        SetAnimationControllerClientRpc(path);
    }

    [Rpc(SendTo.ClientsAndHost)]
    void SetAnimationControllerClientRpc(string path) {
        Animator.runtimeAnimatorController = Resources.Load(path) as RuntimeAnimatorController;
    }



    public void SetAnimation(string animationName) {
        AnimationServerRpc(NetworkObjectId, animationName);
    }

    [Rpc(SendTo.Server)]
    void AnimationServerRpc(ulong sourceNetworkObjectId, string animationName) {
        AnimationClientRpc(sourceNetworkObjectId, animationName);
    }

    [Rpc(SendTo.ClientsAndHost)]
    void AnimationClientRpc(ulong sourceNetworkObjectId, string animationName) {
        Animator.Play(animationName);
    }

    // Update is called once per frame
    void Update() {
        if (!IsOwner) {
            return;
        }

    }
}


