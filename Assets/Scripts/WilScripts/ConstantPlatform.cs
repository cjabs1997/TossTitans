using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantPlatform : MonoBehaviour
{
    public Activator activator;

    public Vector3 startingVelocity;
    public float movementDuration;
    public float timer;
    bool direction;
    Rigidbody2D body;
    private bool activated = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (activator == null)
        {
            activated = true;
        }
        else if (activator.isActive)
        {
            activated = true;
        }

        if (activated)
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
    }
}