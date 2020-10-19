using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sam_Character_Controller : MonoBehaviour
{
    public Sam_Character bigRed;
    public Sam_Character bigBlue;
    public Sam_Character selectedChar;
    public float facingDirection;

    public bool characterGrounded;
    public LayerMask whatisGround;
    public float groundRadius = 0.2f;
    public LayerMask whatisPlayer;

    public CinemachineVirtualCamera followCam;

    void Start()
    {
        selectedChar = bigBlue.GetComponent<Sam_Character>();
        selectedChar.isactive = true;
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
        if (selectedChar.dashTime <= 0)
        {
            selectedChar._rb.velocity = new Vector2(moveInputX * selectedChar.speed, selectedChar._rb.velocity.y);
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
            selectedChar._rb.AddForce(transform.up * selectedChar.jumpForce, ForceMode2D.Impulse);
        }
        if (Input.GetButtonDown("Jump") && !characterGrounded && selectedChar.hasDoubleJump)
        {
            selectedChar._rb.AddForce(transform.up * selectedChar.jumpForce, ForceMode2D.Impulse);
            selectedChar.hasDoubleJump = false;
        }
        if (Input.GetButtonDown("Dash"))
        {
            if (selectedChar.dashCooler <= 0)
            {

                selectedChar._rb.velocity = new Vector2(facingDirection * selectedChar.dashSpeed, selectedChar._rb.velocity.y);
                selectedChar.dashTime = selectedChar.dashLength;
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
                selectedChar.heldObject.GetComponent<Rigidbody2D>().velocity = new Vector2(facingDirection * selectedChar.throwSpeed, 10);
                selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(transform.up * selectedChar.throwSpeed / 2, ForceMode2D.Impulse);
                selectedChar.heldObject.GetComponent<Sam_Character>().isHeld = false;
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
                selectedChar = bigRed.GetComponent<Sam_Character>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
            }            
        }
        else
        {
            if (!bigBlue.isHeld)
            {
                selectedChar.isactive = false;
                selectedChar = bigBlue.GetComponent<Sam_Character>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
            }
           

        }
    }
}

