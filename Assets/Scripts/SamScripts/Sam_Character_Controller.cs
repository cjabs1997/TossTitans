﻿using Cinemachine;
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

                Vector2 force = perpendicularDirection * selectedChar.dashSpeed;
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
        if (Input.GetButtonDown("Jump") && selectedChar.isSwinging)
        {
            selectedChar.isSwinging = false;
            selectedChar.swing._dj.connectedBody = null;
            selectedChar.swing.inUse = false;
            selectedChar.swing = null;
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
            if (selectedChar.dashCooler <= 0 & selectedChar.dashTime <=0)
            {
                selectedChar._rb.velocity = new Vector2(Input.GetAxis("Horizontal") * selectedChar.dashSpeed, Input.GetAxis("Vertical") * selectedChar.dashSpeed);
                selectedChar.dashTime = selectedChar.dashLength;
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
                selectedChar.highlightParticle.SetActive(false);
                selectedChar = bigRed.GetComponent<Sam_Character>();
                followCam.LookAt = selectedChar.transform;
                followCam.Follow = selectedChar.transform;
                selectedChar.isactive = true;
                selectedChar.highlightParticle.SetActive(true);
            }     
            else
            {
                selectedChar.heldObject.GetComponent<Rigidbody2D>().velocity = new Vector2(facingDirection * selectedChar.throwSpeed, 10);
                selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(transform.up * selectedChar.throwSpeed / 2, ForceMode2D.Impulse);
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
                selectedChar.heldObject.GetComponent<Rigidbody2D>().velocity = new Vector2(facingDirection * selectedChar.throwSpeed, 10);
                selectedChar.heldObject.GetComponent<Rigidbody2D>().AddForce(transform.up * selectedChar.throwSpeed / 2, ForceMode2D.Impulse);
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
