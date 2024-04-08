using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : PlayerState {
    public PlayerClimbState(PlayerBase playerBase, PlayerStateMachine stateMachine) : base(playerBase, stateMachine) {
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
    }

    public override void AnimationTriggerEvent(PlayerBase.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
