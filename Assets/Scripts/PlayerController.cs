using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private Transform groundCheck;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    [Header("Ground Detection")]
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 0.9f;
    [SerializeField] private Vector2 crouchOffset = new(0f, -0.45f);
    
    [Header("Game Feel")]
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    private Rigidbody2D rb;

    private PlayerState currentState = PlayerState.Grounded;

    private float standingHeight;
    private Vector2 standingOffset;
    
    private float coyoteTimer;
    private float jumpBufferTimer;
    private bool jumpReleasedBeforeJump;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (collider == null)
        {
            collider = GetComponent<BoxCollider2D>();
        }

        standingHeight = collider.size.y;
        standingOffset = collider.offset;
    }

    private void Update()
    {
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
    }

    private void UpdateState()
    {
        if (currentState == PlayerState.Dead)
            return;

        bool grounded = IsGrounded();

        if (!grounded)
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
        if (!IsGrounded())
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
        collider.size = new Vector2(
            collider.size.x,
            crouchHeight
        );

        collider.offset = crouchOffset;
    }

    private void SetStandingCollider()
    {
        collider.size = new Vector2(
            collider.size.x,
            standingHeight
        );

        collider.offset = standingOffset;
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
    
    private void UpdateCoyoteTimer()
    {
        if (IsGrounded())
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
