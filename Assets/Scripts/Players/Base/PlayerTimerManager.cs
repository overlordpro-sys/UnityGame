using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerTimerManager : NetworkBehaviour
{
    internal float LastPressedJumpTime;
    internal float LastOnGroundTime;

    // Update is called once per frame
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        // Timers
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
    }
}
