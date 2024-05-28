using System.Collections;
using System.Collections.Generic;
using Assets.InputSystem;
using Assets.Scripts.Players;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerTurret : MonoBehaviour {
    [SerializeField] private Player player;

    private Vector2 turretAimVector;

    private bool usingKeyboard = true; // determines whether input vector should be processed as mouse cursor or stick input 
    private bool shootHeld = false;
    private bool mineHeld = false;
    private bool aimHeld = false;

    [SerializeField] private float degreesPerSec = 180;


    [Header("Player Stats")]
    [SerializeField] internal float BulletSizeMult = 1;
    [SerializeField] internal float BulletSpeedMult = 1;
    [SerializeField] internal float BulletAccelerationMult = 1;
    [SerializeField] internal float BulletDamageMult = 1;
    [SerializeField] internal float BulletKnockBackMult = 1;


    void Start() {
        turretAimVector = Vector2.zero;

        player.PlayerInputActions.Player.Shoot.performed += (context => shootHeld = true);
        player.PlayerInputActions.Player.Shoot.canceled += (context => shootHeld = false);

        player.PlayerInputActions.Player.Mine.performed += (context => mineHeld = true);
        player.PlayerInputActions.Player.Mine.canceled += (context => mineHeld = false);

        player.PlayerInputActions.Player.Aim.performed += (context => aimHeld = true);
        player.PlayerInputActions.Player.Aim.canceled += (context => aimHeld = false);
    }

    private void Update() {
        ProcessAimInput();
    }

    private void FixedUpdate() {
        RotateTurret();
    }

    private void ProcessAimInput() {
        Vector2 input = player.PlayerInputActions.Player.Aim.ReadValue<Vector2>();
        if (player.PlayerInput.currentControlScheme.Equals("Keyboard&Mouse")) {
            // Assuming 'input' is the current mouse position in screen coordinates
            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

            // Convert the mouse position to world coordinates
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0; // Make sure to ignore any z-coordinate

            turretAimVector = (mouseWorldPosition - player.Turret.transform.position).normalized;
        }
        else if (player.PlayerInput.currentControlScheme.Equals("Gamepad")) {
            turretAimVector = input.normalized;
        }


    }

    private void RotateTurret() {

        // Calculate the angle to rotate the turret
        float angle = Mathf.Atan2(turretAimVector.y, turretAimVector.x) * Mathf.Rad2Deg;
        angle = Mathf.MoveTowardsAngle(player.Turret.transform.rotation.eulerAngles.z, angle, degreesPerSec * Time.deltaTime);
        // Set the rotation of the turret
        player.Turret.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}