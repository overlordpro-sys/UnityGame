using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState {
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine) {
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void FrameUpdate() {
        base.FrameUpdate();
        if (Player.InputScript.MoveDirection.x != 0) {
            StateMachine.ChangeState(Player.RunState);
        }

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType) {
        base.AnimationTriggerEvent(triggerType);
    }
}
