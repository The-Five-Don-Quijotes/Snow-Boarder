using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float torqueAmount = 0.1f;
    [SerializeField] float boostSpeed = 30f;
    [SerializeField] float baseSpeed = 20f;
    [SerializeField] float jumpForce = 10f; // Jump force amount
    [SerializeField] float gravityForce = 9.8f; // Custom gravity

    Rigidbody2D rb2d;
    SurfaceEffector2D surfaceEffector2D;
    Animator animator; // Reference to Animator
    bool canMove = true;
    bool isGrounded = true; // Check if the player is on the ground

    float totalRotation = 0f; // Track how much the player rotates in the air
    bool isCountingRotation = false;
    CalculateScore scoreManager; // Reference to score manager
    public int bonusPoint = 100;
    Vector2 currentSurfaceNormal = Vector2.up;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        surfaceEffector2D = FindFirstObjectByType<SurfaceEffector2D>();

        // Ensure we have an Animator and avoid null reference error
        //animator = GetComponent<Animator>();
        //if (animator == null)
        //{
        //    Debug.LogError("Animator component is missing on " + gameObject.name);
        //}

        scoreManager = FindFirstObjectByType<CalculateScore>();
        if (scoreManager == null)
        {
            Debug.LogError("CalculateScore script not found in the scene!");
        }
    }

    private void Update()
    {
        if (canMove)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        ApplyCustomGravity();
        if (canMove)
        {
            AdjustSpeed(); // Unified speed control
        }

        if (!isGrounded)
        {
            RotatePlayer();
            TrackRotation();
        }
    }

    float GetSlopeModifier()
    {
        float slopeAngle = Vector2.Angle(currentSurfaceNormal, Vector2.up);
        Debug.Log("Slope: " + slopeAngle);

        if (slopeAngle > 35f) // Assuming slopes over 45 degrees are too steep
        {
            return -1f; // Signal sliding backward
        }

        return Mathf.Clamp(Mathf.Cos(slopeAngle * Mathf.Deg2Rad), 0.5f, 1.5f);
    }

    void AdjustSpeed()
    {
        if (surfaceEffector2D == null) return;

        float slopeModifier = GetSlopeModifier();

        if (slopeModifier < 0f)
        {
            // Apply a consistent sliding-back force when the slope is too steep
            rb2d.AddForce(Vector2.left * baseSpeed, ForceMode2D.Force);
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                surfaceEffector2D.speed = boostSpeed * slopeModifier;
            }
            else
            {
                surfaceEffector2D.speed = baseSpeed * slopeModifier;
            }
        }
    }

    void ApplyCustomGravity()
    {
        rb2d.AddForce(Vector2.down * gravityForce, ForceMode2D.Force);
    }

    public void DisableControls()
    {
        canMove = false;
    }

    //void RespondToBoost()
    //{
    //    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
    //    {
    //        surfaceEffector2D.speed = boostSpeed;
    //    }
    //    else
    //    {
    //        surfaceEffector2D.speed = baseSpeed;
    //    }
    //}

    void RotatePlayer()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rb2d.AddTorque(torqueAmount);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rb2d.AddTorque(-torqueAmount);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpForce); // Corrected velocity

            //// Ensure animator exists before calling SetTrigger
            //if (animator != null)
            //{
            //    animator.SetTrigger("Jump"); // Trigger Jump animation
            //}

            isGrounded = false; // Prevent double jumping
        }
    }

    void TrackRotation()
    {
        if (!isCountingRotation)
        {
            isCountingRotation = true;
            totalRotation = 0f;
        }

        totalRotation += rb2d.angularVelocity * Time.deltaTime;

        if (Mathf.Abs(totalRotation) >= 360f)
        {
            scoreManager.AddScore(bonusPoint); // Award 100 points for a full spin
            totalRotation = 0f; // Reset after scoring
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isCountingRotation = false;
            totalRotation = 0f; // Reset when landing

            currentSurfaceNormal = collision.contacts[0].normal;

            SurfaceEffector2D newSurfaceEffector = collision.gameObject.GetComponent<SurfaceEffector2D>();
            if (newSurfaceEffector != null)
            {
                surfaceEffector2D = newSurfaceEffector;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Properly track when the player leaves the ground
        }

    }
}
