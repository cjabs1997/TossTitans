using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Activator activator;
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
        if (activator == null || activator.isActive)
        {
            timer += direction ? Time.deltaTime : -Time.deltaTime;
            if (timer > movementDuration)
            {
                direction = false;
            }
            if (timer < 0)
            {
                direction = true;
            }
             body.velocity = direction ? startingVelocity : -startingVelocity;
        }
        else
        {
             body.velocity = Vector2.zero;
        }
    }
}