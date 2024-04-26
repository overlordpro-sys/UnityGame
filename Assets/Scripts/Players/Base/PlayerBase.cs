using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerTimerManager))]
[RequireComponent(typeof(PlayerInputScript))]
[RequireComponent(typeof(PlayerColliderScript))]
[RequireComponent(typeof(PlayerMovementData))]
public class PlayerBase : NetworkBehaviour, IDamageable
{
    // Physics objects
    [SerializeField] internal Rigidbody2D Body;
    [SerializeField] internal Collider2D Collider;

    // Child Scripts
    [SerializeField] internal PlayerTimerManager TimerManager;
    [SerializeField] internal PlayerInputScript InputScript;
    [SerializeField] internal PlayerColliderScript ColliderScript;
    [SerializeField] internal PlayerAnimationManager AnimationManager;
    [SerializeField] internal PlayerMovementData MovementData;


    // PlayerBase Stats
    [SerializeField] public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }


    // State Management
    internal PlayerStateMachine StateMachine { get; set; }
    internal PlayerIdleState IdleState { get; set; }
    internal PlayerRunState RunState { get; set; }
    internal PlayerJumpState JumpState { get; set; }
    internal PlayerClimbState ClimbState { get; set; }
    internal PlayerFallingState FallingState { get; set; }


    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        // State initialization
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        RunState = new PlayerRunState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        ClimbState = new PlayerClimbState(this, StateMachine);
        FallingState = new PlayerFallingState(this, StateMachine);
        StateMachine.Initialize(FallingState);

        // Input initialization
        InputScript.JumpAction.action.started +=
            ctx => TimerManager.LastPressedJumpTime = MovementData.jumpInputBufferTime;


        CurrentHealth = MaxHealth;
    }

    internal void SetGravityScale(float scale)
    {
        Body.gravityScale = scale;
    }

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // TODO: Implement death
    }

    // AnimationTriggers
    public enum AnimationTriggerType
    {
        PlayerDamaged,
        PlayFootstepSound,
    }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentPlayerState.AnimationTriggerEvent(triggerType);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        StateMachine.CurrentPlayerState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        StateMachine.CurrentPlayerState.PhysicsUpdate();
    }
}
