using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : KinematicObject
{
    public bool isActive;
    public float runSpeed = 10;
    public float jumpSpeed = 20;
    public float throwSpeed = 30;
    public bool hasGrapplingHook;
    public float dashSpeed = 25;
    public float dashLength = 0.5f;
    public float dashCooldown = 5f;
    public GameObject ally;

    public GameObject highlightParticle;

    public LayerMask whatisPlayer;

    public CinemachineVirtualCamera followCam;


    public float dashTimer = 10000;
    bool hasDoubleJump;
    public bool dash;
    public Vector2 dashDirection;
    bool jump;
    bool thrown;
    bool isHeld;
    Vector2 move;
    SpriteRenderer spriteRenderer;

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
    }

    protected override void Update()
    {
        if (Input.GetButtonDown("Swap/Throw"))
        {
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
            else if (hasDoubleJump && Input.GetButtonDown("Jump"))
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
            else if (Input.GetButtonDown("Dash"))
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
            else if (Input.GetButtonUp("Dash") && hasGrapplingHook)
            {
                dash = false;
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
        if (dash)
        {
            if (hasGrapplingHook)
            {
                Vector3 distance = ally.transform.position - transform.position + new Vector3(0, 0.5f, 0);
                if (distance.magnitude > 1)
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
}