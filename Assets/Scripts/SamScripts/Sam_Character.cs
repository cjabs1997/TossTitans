﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sam_Character : MonoBehaviour
{
    public float speed;
    public float baseSpeed;
    public float jumpForce;
    public float dashSpeed;
    public float throwSpeed;
    public Transform throwpoint;
    public Transform groundCheck;
    public bool hasDoubleJump;
    public float dashLength;
    public float dashTime;
    public float dashCooler;
    public float dashCooldown;
    public bool isactive;
    public bool isHeld;

    public GameObject heldObject;

    public Rigidbody2D _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        hasDoubleJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(heldObject != null)
        {
            heldObject.transform.position = throwpoint.position;
        }
        if(dashTime > 0)
        {
            dashTime -= Time.deltaTime;
            _rb.gravityScale = 0;

        }
        if(dashTime < 0)
        {
            dashCooler = dashCooldown;
            dashTime = 0;
            speed = baseSpeed;
            _rb.gravityScale = 3;
            
        }
        if(dashCooler >= 0)
        {
            
            dashCooler -= Time.deltaTime;
        }
    }
    
}
