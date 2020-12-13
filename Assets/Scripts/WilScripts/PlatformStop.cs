﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStop : Triggerable
{
    public Vector3 startingVelocity;
    public float movementDuration;
    public float timer;
    Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (activated)
        {
            timer += Time.deltaTime;
                
            if (timer > movementDuration)
            {
                body.velocity = Vector2.zero;
            }
            else
            {
                body.velocity = startingVelocity;
            }
        }
    }
}