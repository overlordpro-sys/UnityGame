using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState {
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) {
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        Player.RunScript.Run();
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
