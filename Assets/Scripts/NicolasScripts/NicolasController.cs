using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicolasController : MonoBehaviour
{
    public static NicolasController instance;

    public NicolasCharacter bigRed;
    public NicolasCharacter bigBlue;
    public NicolasCharacter selectedChar;
    public float facingDirection;
    public float lerpValue;
    public float maxVelocityX;

    public bool characterGrounded;
    public LayerMask whatisGround;
    public float groundRadius = 0.2f;
    public LayerMask whatisPlayer;

    public CinemachineVirtualCamera followCam;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        selectedChar = bigBlue.GetComponent<NicolasCharacter>();
        selectedChar.isactive = true;
        selectedChar.highlightParticle.SetActive(true);
        followCam.LookAt = selectedChar.transform;
        followCam.Follow = selectedChar.transform;
    }

    void FixedUpdate()
    {   
        characterGrounded = Physics2D.OverlapCircle(selectedChar.groundCheck.position, groundRadius, whatisGround);
        if (characterGrounded)
        {
            selectedChar.hasDoubleJump = true;
        }
        float moveInputX = Input.GetAxisRaw("Horizontal");
        if (moveInputX > 0)
        {
            facingDirection = 1;
        }
        if (moveInputX < 0)
        {
            facingDirection = -1;
        }

        if (moveInputX == 0 && selectedChar._rb.velocity.y <= 1f && characterGrounded)
        {
            selectedChar._rb.velocity = new Vector2(Mathf.Lerp(selectedChar._rb.velocity.x, 0, lerpValue), selectedChar._rb.velocity.y);
        }             
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Swap/Throw"))
        {
            swapCharacter();
        }
        if (Input.GetButtonDown("Jump") && characterGrounded)
        {
            new Vector2(selectedChar._rb.velocity.x, 0);
            selectedChar._rb.AddForce(transform.up * selectedChar.jumpForce, ForceMode2D.Impulse);
            characterGrounded = false;
        }
        if (Input.GetButtonDown("Jump") && !characterGrounded && selectedChar.hasDoubleJump)
        {
            selectedChar._rb.velocity = new Vector2(selectedChar._rb.velocity.x, 0);
            selectedChar._rb.AddForce(transform.up * selectedChar.jumpForce, ForceMode2D.Impulse);
            selectedChar.hasDoubleJump = false;
        }
        if (Input.GetButtonDown("Dash"))
        {
            if (selectedChar.dashCooler <= 0)
            {
                selectedChar._rb.velocity = Vector2.zero;
                float moveInputX = Input.GetAxisRaw("Horizontal");
                float moveInputY = Input.GetAxisRaw("Vertical");
                selectedChar._rb.AddForce(new Vector2(moveInputX, moveInputY) * selectedChar.dashSpeed/2, ForceMode2D.Impulse);
                selectedChar.dashCooler = selectedChar.dashCooldown;
            }
        }
        if (Input.GetButtonDown("Grab/Throw"))
        {
            if (selectedChar.heldObject == null)
            {
                GameObject throwablePlayer = null;
                Collider2D[] otherPlayer = Physics2D.OverlapCircleAll(selectedChar.transform.position, 1f, whatisPlayer);
                foreach (Collider2D throwable in otherPlayer)
                {
                    if (!throwable.GetComponent<NicolasCharacter>().isactive)
                    {
                        throwablePlayer = throwable.gameObject;
                    }
                }
                if (throwablePlayer != null)
                {
                    throwablePlayer.transform.position = selectedChar.throwpoint.position;
                    selectedChar.heldObject = throwablePlayer;
                    throwablePlayer.GetComponent<NicolasCharacter>().isHeld = true;
                }
            }
            else
            {
                selectedChar.heldObject.GetComponent<Rigidbody2D>().velocity = new Vector2(facingDirection * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x)), 10);
                selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(transform.up * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x)) / 2, ForceMode2D.Impulse);
                selectedChar.heldObject.GetComponent<NicolasCharacter>().isHeld = false;
                selectedChar.heldObject = null;
            }

        }
    }

    void swapCharacter()
    {
        if (selectedChar == bigBlue)
        {
            if (!bigRed.isHeld)
            {               
                selectedChar.isactive = false;
                selectedChar.highlightParticle.SetActive(false);
                selectedChar = bigRed.GetComponent<NicolasCharacter>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
                selectedChar.highlightParticle.SetActive(true);
            }     
            else
            {
                selectedChar.heldObject.GetComponent<Rigidbody2D>().velocity = new Vector2(facingDirection * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x)), 10);
                selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(transform.up * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x)) / 2, ForceMode2D.Impulse);
                selectedChar.heldObject.GetComponent<NicolasCharacter>().isHeld = false;
                selectedChar.heldObject = null;
                selectedChar.isactive = false;
                selectedChar.highlightParticle.SetActive(false);
                selectedChar = bigRed.GetComponent<NicolasCharacter>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
                selectedChar.highlightParticle.SetActive(true);
            }
        }
        else
        {
            if (!bigBlue.isHeld)
            {
                selectedChar.isactive = false;
                selectedChar.highlightParticle.SetActive(false);
                selectedChar = bigBlue.GetComponent<NicolasCharacter>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
                selectedChar.highlightParticle.SetActive(true);
            }
            else
            {
                selectedChar.heldObject.GetComponent<Rigidbody2D>().velocity = new Vector2(facingDirection * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x)), 10);
                selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(transform.up * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x)) / 2, ForceMode2D.Impulse);
                selectedChar.heldObject.GetComponent<NicolasCharacter>().isHeld = false;
                selectedChar.heldObject = null;
                selectedChar.isactive = false;
                selectedChar.highlightParticle.SetActive(false);
                selectedChar = bigBlue.GetComponent<NicolasCharacter>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
                selectedChar.highlightParticle.SetActive(true);
            }
        }
    }
}