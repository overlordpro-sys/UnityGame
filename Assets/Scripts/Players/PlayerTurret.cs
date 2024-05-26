using System.Collections;
using System.Collections.Generic;
using Assets.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerTurret : MonoBehaviour {
    [SerializeField] GameObject turret;

    private PlayerInput playerInput;
    private PlayerControls playerInputActions;
    private Vector2 turretAimVector;

    private bool usingKeyboard = true; // determines whether input vector should be processed as mouse cursor or stick input 
    private bool shootHeld = false;
    private bool mineHeld = false;
    private bool aimHeld = false;

    [SerializeField] private float radPerSec = 180 * Mathf.Deg2Rad;

    void Start() {
        turretAimVector = Vector2.zero;

        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerControls();
        playerInputActions.Player.Shoot.performed += (context => shootHeld = true);
        playerInputActions.Player.Shoot.canceled += (context => shootHeld = false);

        playerInputActions.Player.Mine.performed += (context => mineHeld = true);
        playerInputActions.Player.Mine.canceled += (context => mineHeld = false);

        playerInputActions.Player.Aim.performed += (context => aimHeld = true);
        playerInputActions.Player.Aim.canceled += (context => aimHeld = false);

        playerInputActions.Enable();
    }

    private void Update() {
        ProcessInput();
    }

    private void FixedUpdate() {

    }

    private void ProcessInput() {
        Vector2 input = playerInputActions.Player.Aim.ReadValue<Vector2>();
        if (playerInput.currentControlScheme.Equals("Keyboard&Mouse")) {
            // Assuming 'input' is the current mouse position in screen coordinates
            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

            // Convert the mouse position to world coordinates
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0; // Make sure to ignore any z-coordinate

            turretAimVector = (mouseWorldPosition - turret.transform.position).normalized;
        }
        else if (playerInput.currentControlScheme.Equals("Gamepad")) {
            turretAimVector = input.normalized;
        }

        // Calculate the angle to rotate the turret
        float angle = Mathf.Atan2(turretAimVector.y, turretAimVector.x) * Mathf.Rad2Deg;

        // Set the rotation of the turret
        turret.transform.rotation = Quaternion.Euler(0, 0, angle); // Adjust by -90 degrees if your turret's 'up' is its right

    }
}