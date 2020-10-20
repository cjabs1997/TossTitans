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
        selectedChar.highlight.SetActive(true);
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
        float moveInputX = Input.GetAxisRaw("Horizontal");
        if (moveInputX > 0)
        {
            facingDirection = 1;
        }
        if (moveInputX < 0)
        {
            facingDirection = -1;
        }
        if (selectedChar.dashTime <= 0 && !selectedChar.isSwinging)
        {
            selectedChar._rb.velocity = new Vector2(moveInputX * selectedChar.speed, selectedChar._rb.velocity.y);
        }
        if(selectedChar.isSwinging)
        {
            {
                Vector2 swingPoint = selectedChar.swingPoint.transform.position;
                

                // 1 - Get a normalized direction vector from the player to the hook point
                var playerToHookDirection = (swingPoint - (Vector2)selectedChar.transform.position).normalized;

                // 2 - Inverse the direction to get a perpendicular direction
                Vector2 perpendicularDirection;
                if (moveInputX < 0)
                {
                    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                    var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
                    Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                }
                else
                {
                    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                    var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
                    Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
                }

                var force = perpendicularDirection * selectedChar.speed;
                selectedChar._rb.AddForce(force, ForceMode2D.Force);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Swap/Throw"))
        {
            swapCharacter();
        }
        if(Input.GetButtonDown("Jump") && selectedChar.isSwinging)
            {
                selectedChar.swingPoint.connectedBody = null;
                selectedChar.isSwinging = false;
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

                selectedChar._rb.velocity = new Vector2(facingDirection * selectedChar.dashSpeed, Input.GetAxis("Horizontal") * selectedChar.dashSpeed);
                selectedChar.dashTime = selectedChar.dashLength;
            }
        }
        if (Input.GetButtonDown("Grab/Throw"))
        {
            if(selectedChar.isSwinging)
            {
                selectedChar.swingPoint.connectedBody = null;
                selectedChar.isSwinging = false;
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
                if(throwablePlayer = null)
                {

                }
            }
            else
            {
                selectedChar.heldObject.GetComponent<Rigidbody2D>().velocity = new Vector2(facingDirection * selectedChar.throwSpeed, 10);
                selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(transform.up * selectedChar.throwSpeed / 2, ForceMode2D.Impulse);
                selectedChar.heldObject.GetComponent<Sam_Character>().hasDoubleJump = true;
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
                selectedChar.highlight.SetActive(false);
                selectedChar = bigRed.GetComponent<Sam_Character>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
<<<<<<< Updated upstream
            }
=======
                selectedChar.highlight.SetActive(true);
            }     
>>>>>>> Stashed changes
            else
            {
                selectedChar.heldObject.GetComponent<Rigidbody2D>().velocity = new Vector2(facingDirection * selectedChar.throwSpeed, 10);
                selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(transform.up * selectedChar.throwSpeed / 2, ForceMode2D.Impulse);
                selectedChar.heldObject.GetComponent<Sam_Character>().hasDoubleJump = true;
                selectedChar.heldObject.GetComponent<Sam_Character>().isHeld = false;
                selectedChar.heldObject = null;
                selectedChar.highlight.SetActive(false);
                selectedChar.isactive = false;
                selectedChar = bigRed.GetComponent<Sam_Character>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
                selectedChar.highlight.SetActive(true);
            }
        }
        else
        {
            if (!bigBlue.isHeld)
            {
                selectedChar.isactive = false;
                selectedChar.highlight.SetActive(false);
                selectedChar = bigBlue.GetComponent<Sam_Character>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
                selectedChar.highlight.SetActive(true);
            }
            else
            {
                selectedChar.heldObject.GetComponent<Rigidbody2D>().velocity = new Vector2(facingDirection * selectedChar.throwSpeed, 10);
                selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(transform.up * selectedChar.throwSpeed / 2, ForceMode2D.Impulse);
                selectedChar.heldObject.GetComponent<Sam_Character>().hasDoubleJump = true;
                selectedChar.heldObject.GetComponent<Sam_Character>().isHeld = false;
                selectedChar.heldObject = null;
                selectedChar.isactive = false;
                selectedChar.highlight.SetActive(false);
                selectedChar = bigBlue.GetComponent<Sam_Character>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
                selectedChar.highlight.SetActive(true);
            }
           

        }
    }

    
}

