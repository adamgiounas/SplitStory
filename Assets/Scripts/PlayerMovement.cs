using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [Header("Player Settings")]
    public bool isKing;
    public bool canDoubleJump = true; // Enable or disable double jump
    public bool canGlide = true;

    [Header("Movement Settings")]
    public float speed = 7f;
    public float jumpForce = 10f;
    public float gravityScale = 3f; // Gravity multiplier
    public float glideGravityScale = 0.5f; // Weaker gravity while gliding
    public float turnSpeed = 85f;

    [Header("Jump Timing Enhancements")]
    public float coyoteTime = 0.2f; // Time window to allow jump after leaving ground
    public float jumpBufferTime = 0.2f; // Time window to allow jump input before landing

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isGliding = false;
    private float lastGroundedTime; // Stores last time player was on the ground
    private float lastJumpInputTime; // Stores last time jump was pressed
    private int jumpCount; // Tracks jumps (for double jump)

    private string horizontalAxis;
    private string verticalAxis;
    private string jumpButton;
    private Animator animator;

    void Start() {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();  
        // Assign correct input mappings
        if (isKing) {
            horizontalAxis = "Horizontal_King";
            verticalAxis = "Vertical_King";
            jumpButton = "Jump_King";
        } else {
            horizontalAxis = "Horizontal_Daughter";
            verticalAxis = "Vertical_Daughter";
            jumpButton = "Jump_Daughter";
        }

    }


    void Update() {
        // ✅ Ground check
        isGrounded = controller.isGrounded;

        if (isGrounded) {
            lastGroundedTime = Time.time; // ✅ Store last time the player was on the ground
            jumpCount = 0; // ✅ Reset jump count when grounded
            isGliding = false;
            velocity.y = -2f;
        }
        animator.SetBool("IsGrounded", isGrounded);


        // ✅ Handle movement input
        float moveX = Input.GetAxisRaw(horizontalAxis);
        float moveZ = Input.GetAxisRaw(verticalAxis);
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        if (moveDirection.magnitude < 0.1f)
        {
            moveDirection = Vector3.zero; // ✅ Prevents drifting
        }
        else if (moveDirection.magnitude > 0.1f)
        {
            // ✅ Rotate the character if moving
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection); // ✅ Face movement direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed); // ✅ Smooth rotation
        }
        animator.SetFloat("Speed", moveDirection.magnitude);

        if (Input.GetButtonDown(jumpButton)) {
            lastJumpInputTime = Time.time; // ✅ Store last jump press time
            if (CanJump()) {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
                jumpCount++; // ✅ Increase jump count
                isGliding = false;
                animator.SetTrigger("JumpTrigger");
            }
        }

        if (canGlide && !isKing) {
            if (Input.GetButton(jumpButton) && velocity.y < 0) { // ✅ Holding Jump while falling
                isGliding = true;
            } else {
                isGliding = false;
            }
        }
        // ✅ Apply Gravity 
        if (isGliding) {
            velocity.y += Physics.gravity.y * glideGravityScale * Time.deltaTime; // 🪂 Weak gravity
        } else if (!isGrounded) {
            velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime; // 🌎 Normal gravity
        }
        animator.SetFloat("VerticalVelocity", velocity.y);
        Vector3 verticalMovement = new Vector3(0, velocity.y * Time.deltaTime, 0);
        Vector3 move = moveDirection * speed * Time.deltaTime; // ✅ Move based on world input
        controller.Move(move + verticalMovement);
    }

    private bool CanJump() {
        bool jumpBuffered = lastJumpInputTime + jumpBufferTime > Time.time; // ✅ Jump was pressed recently
        bool groundedJump = isGrounded || Time.time - lastGroundedTime <= coyoteTime; // ✅ Can jump from ground or coyote time
        bool doubleJumpAvailable = jumpCount == 1 && canDoubleJump; // ✅ Can double jump?
        return jumpBuffered && (groundedJump || doubleJumpAvailable);
    }

}
