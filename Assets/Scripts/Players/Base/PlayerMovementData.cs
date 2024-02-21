using UnityEngine;

[CreateAssetMenu(menuName = "PlayerScro[t Run Data")] //Create a new playerData object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the Player
public class PlayerMovementData : ScriptableObject {
    [Header("Gravity")]
    [HideInInspector] public float gravityStrength; //Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.
    [HideInInspector] public float gravityScale; //Strength of the Player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D).
                                                 //Also the value the Player's rigidbody2D.gravityScale is set to.
    [Space(5)]
    public float fallGravityMult; //Multiplier to the Player's gravityScale when falling.
    public float maxFallSpeed; //Maximum fall speed (terminal velocity) of the Player when falling.
    [Space(5)]
    public float fastFallGravityMult; //Larger multiplier to the Player's gravityScale when they are falling and a downwards input is pressed.
                                      //Seen in games such as Celeste, lets the Player fall extra fast if they wish.
    public float maxFastFallSpeed; //Maximum fall speed(terminal velocity) of the Player when performing a faster fall.

    [Header("Run")]
    public float runMaxSpeed; // Target speed we want the Player to reach.
    public float runAcceleration; // Time (approx.) time we want it to take for the Player to accelerate from 0 to the runMaxSpeed.
    [HideInInspector] public float runAccelAmount; // Actual force (multiplied with speedDiff) applied to the Player.
    public float runDecceleration; // Time (approx.) we want it to take for the Player to accelerate from runMaxSpeed to 0.
    [HideInInspector] public float runDeccelAmount; // Actual force (multiplied with speedDiff) applied to the Player .
    [Space(10)]
    [Range(0.01f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
    [Range(0.01f, 1)] public float deccelInAir;
    public bool doConserveMomentum;

    [Space(20)]

    [Header("Jump")]
    public float jumpHeight; //Height of the Player's jump
    public float jumpTimeToApex; //Time between applying the jump force and reaching the desired jump height. These values also control the Player's gravity and jump force.
    [HideInInspector] public float jumpForce; //The actual force applied (upwards) to the Player when they jump.

    [Header("Both Jumps")]
    public float jumpCutGravityMult; //Multiplier to increase gravity if the Player releases thje jump button while still jumping
    [Range(0f, 1)] public float jumpHangGravityMult; //Reduces gravity while close to the apex (desired max height) of the jump
    public float jumpHangTimeThreshold; //Speeds (close to 0) where the Player will experience extra "jump hang". The Player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)
    [Space(0.5f)]
    public float jumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float coyoteTime; //Grace period after falling off a platform, where you can still jump
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.


    private void OnValidate() {
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        //Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        #endregion
    }
}
