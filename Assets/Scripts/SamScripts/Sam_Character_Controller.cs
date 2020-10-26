using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sam_Character_Controller : MonoBehaviour
{
    public static Sam_Character_Controller instance;

    public Sam_Character bigRed;
    public Sam_Character bigBlue;
    public Sam_Character selectedChar;
    public float facingDirection;
    public float lerpValue;
    public float maxVelocityX;
    public float moveInputX;
    public float moveInputY;
    public bool characterGrounded;
    public LayerMask whatisGround;
    public float groundRadius = 0.2f;
    public LayerMask whatisPlayer;
    public bool touchingAlly;
    public float allyCheck = .5f;

    public CinemachineVirtualCamera followCam;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        selectedChar = bigBlue.GetComponent<Sam_Character>();
        selectedChar.isactive = true;
        selectedChar.highlightParticle.SetActive(true);
        followCam.LookAt = selectedChar.transform;
        followCam.Follow = selectedChar.transform;

    }

    void FixedUpdate()
    {
        
        characterGrounded = Physics2D.OverlapCircle(selectedChar.groundCheck.position, groundRadius, whatisGround);
        
        
        if (characterGrounded || selectedChar.isSwinging)
        {
            selectedChar.hasDoubleJump = true;
        }
        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");
        if (moveInputX > 0)
        {
            facingDirection = 1;
        }
        if (moveInputX < 0)
        {
            facingDirection = -1;
        }
        if (!selectedChar.isSwinging && moveInputX !=0)
        {
            if (Mathf.Abs(selectedChar._rb.velocity.x) <= maxVelocityX)
            {
                selectedChar._rb.AddForce(new Vector2(moveInputX * selectedChar.speed, 0), ForceMode2D.Force);                
            }
            else if (characterGrounded)
            {
                selectedChar._rb.velocity = new Vector2(moveInputX * maxVelocityX, selectedChar._rb.velocity.y);
            }
        }

        if (moveInputX == 0 && selectedChar._rb.velocity.y <= 1f && characterGrounded)
        {

            selectedChar._rb.velocity = new Vector2(Mathf.Lerp(selectedChar._rb.velocity.x, 0, lerpValue), selectedChar._rb.velocity.y);
        }   

        if (selectedChar.isSwinging)
        {
                Vector2 ropePoint = selectedChar.swing.transform.position;
                Vector2 playerToHookDirection = (ropePoint - (Vector2)selectedChar.transform.position).normalized;

                // 2 - Inverse the direction to get a perpendicular direction
                Vector2 perpendicularDirection;
                if (moveInputX < 0)
                {
                    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                }
                else
                {
                    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);                                    
                }

                Vector2 force = perpendicularDirection * selectedChar.speed/2;
                selectedChar._rb.AddForce(force, ForceMode2D.Force);
            }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Swap/Throw"))
        {
            swapCharacter();
        }
        if (Input.GetButtonDown("Jump"))
        {
            GameObject vaultablePlayer = null;
            
            if (selectedChar.heldObject == null)
            {
                
                Collider2D[] otherPlayer = Physics2D.OverlapCircleAll(selectedChar.transform.position, .5f, whatisPlayer);
                foreach (Collider2D vaultable in otherPlayer)
                {
                    if (!vaultable.GetComponent<Sam_Character>().isactive)
                    {
                        vaultablePlayer = vaultable.gameObject;
                    }
                }
                Debug.Log(vaultablePlayer);
                if (vaultablePlayer != null)
                {
                    Vector2 vaultVector = new Vector2(moveInputX, moveInputY);
                    selectedChar._rb.AddForce(vaultVector * ((vaultablePlayer.GetComponent<Sam_Character>().throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x) * 10)), ForceMode2D.Impulse);
                    
                }
            }
            if (vaultablePlayer == null)
            {
                if (selectedChar.isSwinging)
                {
                    selectedChar.isSwinging = false;
                    selectedChar.swing._dj.connectedBody = null;
                    selectedChar.swing.inUse = false;
                    selectedChar.swing = null;
                    Vector2 throwVector = new Vector2(moveInputX, moveInputY);
                    selectedChar._rb.AddForce(throwVector * (selectedChar.speed + Mathf.Abs(selectedChar._rb.velocity.x) * 10), ForceMode2D.Impulse);
                }
                if (vaultablePlayer == null && characterGrounded)
                {
                    selectedChar._rb.velocity = new Vector2(selectedChar._rb.velocity.x, 0);
                    selectedChar._rb.AddForce(transform.up * selectedChar.jumpForce, ForceMode2D.Impulse);

                }
                if (vaultablePlayer == null && !characterGrounded && selectedChar.hasDoubleJump)
                {
                    selectedChar._rb.velocity = new Vector2(selectedChar._rb.velocity.x, 0);
                    selectedChar._rb.AddForce(transform.up * selectedChar.jumpForce, ForceMode2D.Impulse);
                    selectedChar.hasDoubleJump = false;
                }
            }
            
        }
        if (Input.GetButtonDown("Dash"))
        {
            if (selectedChar.dashCooler <= 0)
            {
                selectedChar._rb.velocity = Vector2.zero;
                float moveInputX = Input.GetAxisRaw("Horizontal");
                float moveInputY = Input.GetAxisRaw("Vertical");
                selectedChar._rb.AddForce(new Vector2(moveInputX, moveInputY) * selectedChar.dashSpeed, ForceMode2D.Impulse);
                selectedChar.dashCooler = selectedChar.dashCooldown;
            }
        }
        if (Input.GetButtonDown("Grab/Throw"))
        {
            if(selectedChar.isSwinging)
            {
                selectedChar.isSwinging = false;
                selectedChar.swing._dj.connectedBody = null;
                selectedChar.swing.inUse = false;
                selectedChar.swing = null;
                Vector2 throwVector = new Vector2(moveInputX, moveInputY);
                selectedChar._rb.AddForce(throwVector * (selectedChar.speed + Mathf.Abs(selectedChar._rb.velocity.x) * 10), ForceMode2D.Impulse);
            }
            if (selectedChar.heldObject == null)
            {
                GameObject throwablePlayer = null;
                Collider2D[] otherPlayer = Physics2D.OverlapCircleAll(selectedChar.transform.position, 1f, whatisPlayer);
                foreach (Collider2D throwable in otherPlayer)
                {
                    if (!throwable.GetComponent<Sam_Character>().isactive)
                    {
                        throwablePlayer = throwable.gameObject;
                    }
                }
                if (throwablePlayer != null)
                {
                    throwablePlayer.transform.position = selectedChar.throwpoint.position;
                    selectedChar.heldObject = throwablePlayer;
                    throwablePlayer.GetComponent<Sam_Character>().isHeld = true;
                }
            }
            else
            {
                if (moveInputX != 0 && moveInputY != 0)
                {
                    Vector2 throwVector = new Vector2(moveInputX, moveInputY);

                    selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(throwVector * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x) * 10), ForceMode2D.Impulse);
                    selectedChar.heldObject.GetComponent<Sam_Character>().isHeld = false;
                    selectedChar.heldObject = null;
                }
                if(moveInputX == 0 && moveInputY!= 0)
                {
                    Vector2 throwVector = new Vector2(selectedChar.throwpoint.position.y + facingDirection, moveInputY);
                    selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(throwVector * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x) * 10), ForceMode2D.Impulse);
                    selectedChar.heldObject.GetComponent<Sam_Character>().isHeld = false;
                    selectedChar.heldObject = null;
                }
                if (moveInputX == 0 && moveInputY == 0)
                {
                    Vector2 throwVector = new Vector2(selectedChar.throwpoint.position.y + facingDirection, selectedChar.throwpoint.position.y + 1);
                    selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(throwVector * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x) * 10), ForceMode2D.Impulse);
                    selectedChar.heldObject.GetComponent<Sam_Character>().isHeld = false;
                    selectedChar.heldObject = null;
                }
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
                selectedChar = bigRed.GetComponent<Sam_Character>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
                selectedChar.highlightParticle.SetActive(true);
            }     
            else
            {
                selectedChar.heldObject.GetComponent<Rigidbody2D>().velocity = new Vector2(facingDirection * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x)), 10);
                selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(transform.up * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x)) / 2, ForceMode2D.Impulse);
                selectedChar.heldObject.GetComponent<Sam_Character>().isHeld = false;
                selectedChar.heldObject = null;
                selectedChar.isactive = false;
                selectedChar.highlightParticle.SetActive(false);
                selectedChar = bigRed.GetComponent<Sam_Character>();
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
                selectedChar = bigBlue.GetComponent<Sam_Character>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
                selectedChar.highlightParticle.SetActive(true);
            }
            else
            {
                selectedChar.heldObject.GetComponent<Rigidbody2D>().velocity = new Vector2(facingDirection * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x)), 10);
                selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(transform.up * (selectedChar.throwSpeed + Mathf.Abs(selectedChar._rb.velocity.x)) / 2, ForceMode2D.Impulse);
                selectedChar.heldObject.GetComponent<Sam_Character>().isHeld = false;
                selectedChar.heldObject = null;
                selectedChar.isactive = false;
                selectedChar.highlightParticle.SetActive(false);
                selectedChar = bigBlue.GetComponent<Sam_Character>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
                selectedChar.highlightParticle.SetActive(true);
            }


        }
    }
}

