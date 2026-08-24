using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Rigidbody2D rb;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    [Header("Ground Detection")]
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 0.9f;
    [SerializeField] private Vector2 crouchOffset = new(0f, -0.45f);
    
    [Header("Fast Fall")]
    [SerializeField] private float fastFallSpeed = 18f;
    
    [Header("Game Feel")]
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    private PlayerState currentState = PlayerState.Grounded;

    private bool isGrounded;

    private float standingHeight;
    private Vector2 standingOffset;
    
    private float coyoteTimer;
    private float jumpBufferTimer;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (playerCollider == null)
        {
            playerCollider = GetComponent<BoxCollider2D>();
        }

        standingHeight = playerCollider.size.y;
        standingOffset = playerCollider.offset;
    }

    private void Update()
    {
        isGrounded = IsGrounded();
        
        UpdateCoyoteTimer();
        UpdateJumpBuffer();
        UpdateState();
        HandleCrouch();
        if (playerInput.JumpReleased)
        {
            CutJump();
        }
    }

    private void FixedUpdate()
    {
        HandleJump();
        HandleFastFall();
    }

    private void UpdateState()
    {
        if (currentState == PlayerState.Dead)
            return;

        if (!isGrounded)
        {
            currentState = rb.velocity.y > 0f ? PlayerState.Jumping : PlayerState.Falling;

            return;
        }

        if (playerInput.CrouchHeld)
        {
            currentState = PlayerState.Crouching;
            return;
        }

        currentState = PlayerState.Grounded;
    }

    private void HandleJump()
    {
        if (jumpBufferTimer <= 0f)
            return;

        if (coyoteTimer <= 0f)
            return;

        rb.velocity = new Vector2(
            rb.velocity.x,
            jumpForce
        );

        currentState = PlayerState.Jumping;
        
        jumpBufferTimer = 0f;
        coyoteTimer = 0f;
        
        SetStandingCollider();

        if (!playerInput.JumpHeld)
        {
            CutJump();
        }
    }

    private void HandleCrouch()
    {
        if (!isGrounded)
        {
            SetStandingCollider();
            return;
        }
        
        if (currentState == PlayerState.Crouching)
        {
            SetCrouchCollider();
        }
        else
        {
            SetStandingCollider();
        }
    }

    private void SetCrouchCollider()
    {
        playerCollider.size = new Vector2(
            playerCollider.size.x,
            crouchHeight
        );

        playerCollider.offset = crouchOffset;
    }

    private void SetStandingCollider()
    {
        playerCollider.size = new Vector2(
            playerCollider.size.x,
            standingHeight
        );

        playerCollider.offset = standingOffset;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }
    
    private void CutJump()
    {
        if (rb.velocity.y <= 0f)
            return;

        rb.velocity = new Vector2(
            rb.velocity.x,
            rb.velocity.y * jumpCutMultiplier
        );
    }
    
    private void HandleFastFall()
    {
        if (!playerInput.CrouchHeld || IsGrounded())
            return;

        if (rb.velocity.y < 0f)
        {
            rb.velocity = new Vector2(
                rb.velocity.x,
                Mathf.Min(rb.velocity.y, -fastFallSpeed)
            );
        }
    }
    
    private void UpdateCoyoteTimer()
    {
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
    }
    
    private void UpdateJumpBuffer()
    {
        if (playerInput.JumpPressed)
        {
            jumpBufferTimer = jumpBufferTime;
        }

        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.deltaTime;
        }
    }
}
