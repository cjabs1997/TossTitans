using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : KinematicObject
{
    public bool allowDoubleJump ;
    public bool allowDash;
    public bool hasGrapplingHook;
    public bool allowIceTransform;
    public bool allowJumpThrouhHoops;
    public bool allowFly;
    public bool isActive;
    public float runSpeed = 10;
    public float jumpSpeed = 20;
    public float throwSpeed = 30;
    public float dashSpeed = 25;
    public float dashLength = 0.5f;
    public float dashCooldown = 5f;
    public float fireHoopSpeed = 40;
    public Vector3 checkpointPosition;
    public GameObject ally;
    public GameObject highlightParticle;
    public LayerMask whatisPlayer;
    public LayerMask whatisGround;
    public CinemachineVirtualCamera followCam;

    float dashTimer = 1000000;
    bool hasDoubleJump;
    bool dash;
    Vector2 dashDirection;
    bool jump;
    bool thrown;
    bool isHeld;
    bool isIce;
    bool isDead;
    bool isInsideHoop;
    float deadTimer;
    float deadTimeCooldown = 0.2f;
    Vector2 move;
    SpriteRenderer spriteRenderer; 
    Animator animator;
    Interactable interactable;

    public void JumpThroughFire(Vector3 checkpointPosition)
    {
        if (allowJumpThrouhHoops)
        {
            isInsideHoop = true;
        }
        else
        {
            Kill(checkpointPosition);
        }
    }

    public void Kill(Vector3 checkpointPosition)
    {
        Teleport(checkpointPosition);
        isDead = true;
        deadTimer = 0;
        isHeld = false;
        isIce = false;
        dash = false;
        hasDoubleJump = false;
        jump = false;
        if (isIce)
        {
            isIce = false;
            gravity /= 4;
        }
    }

    protected override void Start()
    {
        base.Start();
        ActivateCharacter();
    }

    bool CanBeThrown()
    {
        GameObject throwablePlayer = null;
        Collider2D[] otherPlayer = Physics2D.OverlapCircleAll(transform.position, 2f, whatisPlayer);
        foreach (Collider2D throwable in otherPlayer)
        {
            if (!throwable.GetComponent<CharacterController>().isActive)
            {
                return true;
            }
        }
        return false;
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
        interactable = this.GetComponent<Interactable>();
    }

    protected override void Update()
    {
        if (Input.GetButtonDown("Swap/Throw"))
        {
            isHeld = allowFly;
            isActive = !isActive;
            ActivateCharacter();
        }
        if (isDead)
        {
            deadTimer += Time.deltaTime;
            if (deadTimer > deadTimeCooldown)
            {
                isDead = false;
            }
            else
            {
                move = Vector2.zero;
                velocity = new Vector2(0, -10);
                targetVelocity = new Vector2(0, -10);
                return;
            }
        }
        if (isActive)
        {
            if (IsGrounded)
            {
                hasDoubleJump = true;
            }
            if (!hasGrapplingHook)
            {
                dashTimer += Time.deltaTime;
                if (dashLength <= dashTimer)
                {
                    dash = false;
                }
            }

            move.x = Input.GetAxis("Horizontal");
            move.y = Input.GetAxis("Vertical");
            if (IsGrounded && Input.GetButtonDown("Jump") && !isIce)
            {
                jump = true;
            }
            else if (allowDoubleJump && hasDoubleJump && Input.GetButtonDown("Jump") && !isIce)
            {
                jump = true;
                hasDoubleJump = false;
            }
            else if (Input.GetButtonDown("Grab/Throw") && CanBeThrown())
            {
                isHeld = true;
            }
            else if (Input.GetButtonUp("Grab/Throw") && isHeld)
            {
                thrown = true;
                isHeld = false;
                hasDoubleJump = true;
            }
            else if (allowDash && Input.GetButtonDown("Dash"))
            {
                if(hasGrapplingHook)
                {
                    dash = true;
                }
                else if (dashTimer >= dashCooldown)
                {
                    dash = true;
                    dashTimer = 0;
                    dashDirection = (Vector2)Vector3.Normalize(move);
                }
            }
            else if (allowDash && Input.GetButtonUp("Dash") && hasGrapplingHook)
            {
                dash = false;
            }
            else if (Input.GetButtonDown("Ice Transform") && allowIceTransform)
            {
                if (isIce)
                {
                    isIce = false;
                    gravity /= 4;
                }
                else
                {
                    isIce = true;
                    gravity *= 4;
                }
            }
        }
        else
        {
            move.x = 0;
        }
        base.Update();
    }

    protected override bool CanDestroyGround()
    {
        return isIce && velocity.y < -10;
    }

    protected override void ComputeVelocity()
    {   
        targetVelocity = Vector2.zero;
        if(isInsideHoop)
        {
            dash = false;
            velocity.y = fireHoopSpeed / 5;
            targetVelocity.y = fireHoopSpeed / 5;
            if(velocity.x >= 0)
            {
                velocity.x = fireHoopSpeed;
                targetVelocity.x = fireHoopSpeed;
            }
            if(velocity.x < 0)
            {
                velocity.x = -fireHoopSpeed;
                targetVelocity.x = -fireHoopSpeed;
            }
            isInsideHoop = false;
            return;
        }
        if (isIce)
        {
            velocity.x = 0;
            targetVelocity.x = 0;
            return;
        }
        if (dash)
        {
            if (hasGrapplingHook)
            {
                Vector3 distance = ally.transform.position - transform.position + new Vector3(0, 0.5f, 0);
                Vector2 direction =  (Vector2)Vector3.Normalize(distance);
                var hasSight = Physics2D.Raycast((Vector2)(transform.position), direction, distance.magnitude, whatisGround.value).collider == null;

                if (hasSight && distance.magnitude > 2)
                {
                    velocity = (Vector2)Vector3.Normalize(distance) * dashSpeed;
                    targetVelocity = (Vector2)Vector3.Normalize(distance) * dashSpeed;
                    return;
                }
                else
                {
                    dash = false;
                }
            }
            else
            {
                velocity = dashDirection * dashSpeed;
                targetVelocity = dashDirection * dashSpeed;
            }
        }
        if (jump)
        {
            velocity.y = jumpSpeed;
            targetVelocity.y = jumpSpeed;
            jump = false;
            return;
        }

        if (thrown)
        {
            velocity = move * throwSpeed;
            targetVelocity = move * throwSpeed;
            thrown = false;
            return;
        }

        if (isHeld)
        {
            Vector3 distance = ally.transform.position - transform.position + new Vector3(0, 0.5f, 0);
            velocity = distance * 10f;
            targetVelocity = distance * 10f;
            return;
        }

        if (move.x > 0.01f)
            spriteRenderer.flipX = false;
        else if (move.x < -0.01f)
            spriteRenderer.flipX = true;

        targetVelocity.x = move.x * runSpeed;
    }

    void ActivateCharacter()
    {
        if (isActive)
        {
            highlightParticle.SetActive(true);
            followCam.LookAt = transform;
            followCam.Follow = transform;
        }
        else
        {
            highlightParticle.SetActive(false);
        }
    }

    public bool GetHasDoubleJump()
    {
        return hasDoubleJump;
    }

    public bool GetDash()
    {
        return dash;
    }

    public bool GetIsIce()
    {
        return isIce;
    }
}