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
    public GameObject ally;
    public GameObject highlightParticle;
    public LayerMask whatisPlayer;
    public CinemachineVirtualCamera followCam;

    public float dashTimer = 1000000;
    bool hasDoubleJump;
    bool dash;
    public Vector2 dashDirection;
    bool jump;
    bool thrown;
    bool isHeld;
    bool isIce;
    Vector2 move;
    SpriteRenderer spriteRenderer; 
    Animator animator;
    Interactable interactable;

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
            if (IsGrounded && Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
            else if (allowDoubleJump && hasDoubleJump && Input.GetButtonDown("Jump"))
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

    protected override void ComputeVelocity()
    {
        targetVelocity = Vector2.zero;
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
                var count = body.Cast(direction, contactFilter, hitBuffer, distance.magnitude);
                bool hasSight = true;
                for (var i = 0; i < count; i++)
                {
                    hasSight = false;
                }
                if (hasSight && distance.magnitude > 1)
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
}