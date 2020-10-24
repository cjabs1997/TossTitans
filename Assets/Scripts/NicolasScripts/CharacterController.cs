using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : KinematicObject
{
    public float runSpeed = 10;
    public float jumpSpeed = 20;
    public float dashSpeed = 3;
    public float throwSpeed;
    public Transform groundCheck;
    public bool hasDoubleJump;

    public GameObject ally;

    public GameObject highlightParticle;

    public bool isActive;
    public bool isHeld;
    public GameObject heldObject;
    public Rigidbody2D rb;

    public LayerMask whatisPlayer;

    public CinemachineVirtualCamera followCam;

    public Collider2D collider2d;
    bool dash;
    bool jump;
    Vector2 move;
    SpriteRenderer spriteRenderer;

    public Bounds Bounds => collider2d.bounds;


    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        ActivateCharacter();
    }

    // void Update()
    // {

    //     // if (heldObject != null)
    //     // {
    //     //     heldObject.transform.position = this.transform.position + new Vector3(0,1,0);
    //     // }
    //     // if(dashCooler >= 0)
    //     // {     
    //     //     dashCooler -= Time.deltaTime;
    //     // }

    //     if (!isactive)
    //     {
    //         if (IsGrounded)
    //         {
    //             rb.velocity.x = 0;
    //         }
    //     }

    // if (Input.GetButtonDown("Dash"))
    // {
    //     if (selectedChar.dashCooler <= 0)
    //     {
    //         selectedChar._rb.velocity = Vector2.zero;
    //         float moveInputX = Input.GetAxisRaw("Horizontal");
    //         float moveInputY = Input.GetAxisRaw("Vertical");
    //         selectedChar._rb.AddForce(new Vector2(moveInputX, moveInputY) * selectedChar.dashSpeed/2, ForceMode2D.Impulse);
    //         selectedChar.dashCooler = selectedChar.dashCooldown;
    //     }
    // }
    // if (Input.GetButtonDown("Grab/Throw"))
    // {
    //     if (selectedChar.heldObject == null)
    //     {
    //         GameObject throwablePlayer = null;
    //         Collider2D[] otherPlayer = Physics2D.OverlapCircleAll(selectedChar.transform.position, 1f, whatisPlayer);
    //         foreach (Collider2D throwable in otherPlayer)
    //         {
    //             if (!throwable.GetComponent<NicolasCharacter>().isactive)
    //             {
    //                 throwablePlayer = throwable.gameObject;
    //             }
    //         }
    //         if (throwablePlayer != null)
    //         {
    //             throwablePlayer.transform.position = new Vector3(0,1,0);
    //             selectedChar.heldObject = throwablePlayer;
    //             throwablePlayer.GetComponent<NicolasCharacter>().isHeld = true;
    //         }
    //     }
    //     else
    //     {
    //         selectedChar.heldObject.GetComponent<Rigidbody2D>().velocity = new Vector2(facingDirection * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x)), 10);
    //         selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(transform.up * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x)) / 2, ForceMode2D.Impulse);
    //         selectedChar.heldObject.GetComponent<NicolasCharacter>().isHeld = false;
    //         selectedChar.heldObject = null;
    //     }

    // }

    // }

    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
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
            move.x = Input.GetAxis("Horizontal");
            if (IsGrounded && Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
            else if (hasDoubleJump && Input.GetButtonDown("Jump"))
            {
                jump = true;
                hasDoubleJump = false;
            }
            else if (Input.GetButtonDown("Dash"))
            {
                dash = true;
                jump = false;
            }
            else if (Input.GetButtonUp("Dash"))
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
        if (dash)
        {
            Vector3 distance = ally.transform.position - transform.position + new Vector3(0, 0.5f, 0);
            if(distance.magnitude > 1)
            {
                velocity = (Vector2) Vector3.Normalize(distance) * dashSpeed;
                return;
            }     
        }

        if (jump)
        {
            velocity.y = jumpSpeed;
            jump = false;
        }

        if (move.x > 0.01f)
            spriteRenderer.flipX = false;
        else if (move.x < -0.01f)
            spriteRenderer.flipX = true;

        velocity.x = move.x * runSpeed;
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