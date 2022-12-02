using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//This line validates that any GameObject attached to this script has the minimum
//necessary components required to work
[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D),
typeof(Animator))]
public class SoyBoyController : MonoBehaviour
{
    public float jump = 14f;
    public float airAccel = 3f;

    public float jumpDurationThreshold = 0.25f;
    private float jumpDuration;

    public bool isJumping;
    public float jumpSpeed = 8f;
    private float rayCastLengthCheck = 0.005f;
    private float width;
    private float height;

    public float speed = 14f;
    public float accel = 6f;
    private Vector2 input;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        width = GetComponent<Collider2D>().bounds.extents.x + 0.1f;
        height = GetComponent<Collider2D>().bounds.extents.y + 0.2f;
    }
    void Start()
    {

    }
    public bool PlayerIsOnGround()
    {
        // The first ground check performs a Raycast directly below the center of the the
        // character(Transform.position.x), using a length equal to the value of
        // rayCastLengthCheck which is defaulted to 0.005f
        bool groundCheck1 = Physics2D.Raycast(new Vector2(
        transform.position.x, transform.position.y - height),
        -Vector2.up, rayCastLengthCheck);
        bool groundCheck2 = Physics2D.Raycast(new Vector2(
        transform.position.x + (width - 0.2f),
        transform.position.y - height), -Vector2.up,
        rayCastLengthCheck);
        bool groundCheck3 = Physics2D.Raycast(new Vector2(
        transform.position.x - (width - 0.2f),
        transform.position.y - height), -Vector2.up,
        rayCastLengthCheck);
        // If any of the ground checks come back as true, then this method returns true to the
        //caller.Otherwise, it will return false.
        if (groundCheck1 || groundCheck2 || groundCheck3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsWallToLeftOrRight()
    {
        // implicit bool conversion check of the Physics2D.Raycast()
        //method to see if either of two raycasts sent out to the left(-Vector2.right) and to
        //the right(Vector2.right) of the character sprite hit anything.
        bool wallOnleft = Physics2D.Raycast(new Vector2(
        transform.position.x - width, transform.position.y),
        -Vector2.right, rayCastLengthCheck);
        bool wallOnRight = Physics2D.Raycast(new Vector2(
        transform.position.x + width, transform.position.y), 
        Vector2.right, rayCastLengthCheck);
        // If either of these raycasts hit anything, the method returns true — otherwise, false
        if (wallOnleft || wallOnRight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //This method will return true if the character is either on the ground, or has a wall to the left or right of them.
    public bool PlayerIsTouchingGroundOrWall()
    {
        if (PlayerIsOnGround() || IsWallToLeftOrRight())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public int GetWallDirection()
    {
        bool isWallLeft = Physics2D.Raycast(new Vector2(
        transform.position.x - width, transform.position.y),
        -Vector2.right, rayCastLengthCheck);
        bool isWallRight = Physics2D.Raycast(new Vector2(
        transform.position.x + width, transform.position.y),
        Vector2.right, rayCastLengthCheck);
        if (isWallLeft)
        {
            return -1;
        }
        else if (isWallRight)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    void Update()
    {
        // Input.GetAxis() gets X and Y values from the built-in Unity control
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Jump");
        // If input.x is greater than 0, then the player is facing right, so the sprite gets flipped
        //on the X - axis
        if (input.x > 0f)
        {
            sr.flipX = false;
        }
        else if (input.x < 0f)
        {
            sr.flipX = true;
        }
        //As long as the jump button is held down, jumpDuration is counted up using the time the
        //previous frame took to complete(Time.deltaTime
        if (input.y >= 1f)
        {
            jumpDuration += Time.deltaTime;
        }
        else
        {
            isJumping = false;
            jumpDuration = 0f;
        }

        if (PlayerIsOnGround() && isJumping == false)
        {
            if (input.y > 0f)
            {
                isJumping = true;
            }
        }
        //Jump is cancelled if held longer than the Threshold
        if (jumpDuration > jumpDurationThreshold) input.y = 0f;
    }

    void FixedUpdate()
    {
        // Assign the value of accel — the public float field — to a private variable named
        // acceleration.
        var acceleration = 0f;
        if (PlayerIsOnGround())
        {
            acceleration = accel;
        }
        else
        {
            acceleration = airAccel;
        }
        var xVelocity = 0f;
        // If horizontal axis controls are neutral, then xVelocity is set to 0
        if (PlayerIsOnGround() && input.x == 0)
        {
            xVelocity = 0f;
        }
        else
        {
            xVelocity = rb.velocity.x;
            //ensures that the yVelocity value is set to the jump value of 14 when the character is jumping from the ground, or from a wall
            var yVelocity = 0f;
            if (PlayerIsTouchingGroundOrWall() && input.y == 1)
            {
                yVelocity = jump;
            }
            else
            {
                yVelocity = rb.velocity.y;
            }

            // Force is added to rb by calculating the current value of the horizontal axis controls
            //multiplied by speed, which is in turn multiplied by acceleration
            rb.AddForce(new Vector2(((input.x * speed) - rb.velocity.x)
            * acceleration, 0));
            // Velocity is reset on rb so it can stop Super Soy Boy from moving left or right when
            //controls are in a neutral state.
            rb.velocity = new Vector2(xVelocity, yVelocity);

            if (IsWallToLeftOrRight() && !PlayerIsOnGround() && input.y == 1)
            {
                rb.velocity = new Vector2(-GetWallDirection()
                * speed * 0.75f, rb.velocity.y);
            }
        }
        //If pressed less the Threshold, it gives a new velocity for y
        if (isJumping && jumpDuration < jumpDurationThreshold)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
}
