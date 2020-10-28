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
    Vector3 velocity;

    void Update()
    {
        timer += direction ? Time.deltaTime : -Time.deltaTime;
        if (timer > movementDuration)
        {
            direction = false;
            velocity = -startingVelocity;
        }
        if (timer < 0)
        {
            direction = true;
            velocity = startingVelocity;
        }
        transform.position += velocity * Time.deltaTime;
    }
}