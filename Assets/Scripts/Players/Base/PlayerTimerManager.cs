using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerTimerManager : NetworkBehaviour {
    public float LastPressedJumpTime;
    public float LastOnGroundTime;

    // Update is called once per frame
    void Update() {
        if (!IsOwner) {
            return;
        }

        // Timers
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
    }
}
