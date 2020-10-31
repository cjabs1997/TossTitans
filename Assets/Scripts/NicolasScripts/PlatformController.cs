using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Vector3 startingVelocity;
    public float movementDuration;
    public float timer;
    bool direction;
    Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer += direction ? Time.deltaTime : -Time.deltaTime;
        if (timer > movementDuration)
        {
            direction = false;
            body.velocity = -startingVelocity;
        }
        if (timer < 0)
        {
            direction = true;
            body.velocity = startingVelocity;
        }
    }
}