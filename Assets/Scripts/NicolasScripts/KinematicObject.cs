﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Implements game physics for some in game entity.
/// </summary>
public class KinematicObject : MonoBehaviour
{
    /// <summary>
    /// The minimum normal (dot product) considered suitable for the entity sit on.
    /// </summary>
    public float minGroundNormalY = .65f;

    /// <summary>
    /// A custom gravity coefficient applied to this entity.
    /// </summary>
    public float gravity = 1f;

    public float groundControl = 250;

    public float aerialControl = 50;

    /// <summary>
    /// The current velocity of the entity.
    /// </summary>
    public Vector2 velocity;

    /// <summary>
    /// Is the entity currently sitting on a surface?
    /// </summary>
    /// <value></value>
    public bool IsGrounded { get; private set; }

    protected Vector2 targetVelocity;
    protected Vector2 groundNormal;
    protected Rigidbody2D body;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.1f;

    Rigidbody2D groundBody;


    /// <summary>
    /// Bounce the object's vertical velocity.
    /// </summary>
    /// <param name="value"></param>
    public void Bounce(float value)
    {
        velocity.y = value;
    }

    /// <summary>
    /// Bounce the objects velocity in a direction.
    /// </summary>
    /// <param name="dir"></param>
    public void Bounce(Vector2 dir)
    {
        velocity.y = dir.y;
        velocity.x = dir.x;
    }

    /// <summary>
    /// Teleport to some position.
    /// </summary>
    /// <param name="position"></param>
    public void Teleport(Vector3 position)
    {
        body.position = position;
        velocity *= 0;
        body.velocity *= 0;
    }

    protected virtual void OnEnable()
    {
        body = GetComponent<Rigidbody2D>();
        body.isKinematic = true;
    }

    protected virtual void OnDisable()
    {
        body.isKinematic = false;
    }

    protected virtual void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    protected virtual void Update()
    {
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    protected virtual bool CanDestroyGround()
    {
        return false;
    }

    protected virtual void FixedUpdate()
    {        
        if (targetVelocity.y == 0)
        {
            velocity += Vector2.down * gravity * Time.deltaTime;        
        }
        else
        {
            velocity.y = targetVelocity.y;
        }

        float control = IsGrounded ? groundControl : aerialControl;

        if (targetVelocity.x > velocity.x)
        {
            velocity.x = Mathf.Min(targetVelocity.x, velocity.x + control * Time.deltaTime);
        }
        else if (targetVelocity.x < velocity.x)
        {
            velocity.x = Mathf.Max(targetVelocity.x, velocity.x - control * Time.deltaTime);
        }

        Vector2 velocityPlatform = Vector2.zero;


        TestGrounded();
        
        if(IsGrounded)
        {
            velocityPlatform = groundBody.velocity;
        }

        IsGrounded = false;
        
        var deltaPosition = (velocity)* Time.deltaTime;

        var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        var move = moveAlongGround * deltaPosition.x + (velocityPlatform.x * Vector2.right * Time.deltaTime);

        PerformMovement(move, false);

        move = Vector2.up * (deltaPosition.y);

       
        PerformMovement(move, true);
    }

    void TestGrounded()
    {      
        var count = body.Cast(new Vector2(0, -0.1f), contactFilter, hitBuffer, 0.1f);
            for (var i = 0; i < count; i++)
            {
                var currentNormal = hitBuffer[i].normal;             
                if (currentNormal.y > minGroundNormalY)
                {
                    IsGrounded = true;
                    groundBody = hitBuffer[i].rigidbody;
                }
            }
        
    }

    void PerformMovement(Vector2 move, bool yMovement)
    {
        var distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            //check if we hit anything in current direction of travel
            var count = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            for (var i = 0; i < count; i++)
            {
                if (CanDestroyGround() && hitBuffer[i].collider.gameObject.GetComponent<Destroyable>())
                {
                    Destroy(hitBuffer[i].collider.gameObject);
                }
                else
                {
                     var currentNormal = hitBuffer[i].normal;
            
                    //is this surface flat enough to land on?
                    if (currentNormal.y > minGroundNormalY)
                    {
                        IsGrounded = true;
                        // if moving up, change the groundNormal to new surface normal.
                        if (yMovement)
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }
                    if(IsGrounded)
                    {
                        velocity.y = 0;
                    }
                    if (!IsGrounded)
                    {                             
                        //We are airborne, but hit something, so cancel vertical up and horizontal velocity.
                        if (yMovement)
                        {
                            velocity.y = Mathf.Min(velocity.y, 0);
                        }
                        else
                        {
                            velocity.x *= 0;
                        }    
                    }

                    //remove shellDistance from actual move distance.
                    var modifiedDistance = hitBuffer[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                    if (yMovement)
                    {
                        if (move.y >= 0)
                        {
                            distance += hitBuffer[i].rigidbody.velocity.y * Time.deltaTime;
                        }
                        else
                        {
                            distance -= hitBuffer[i].rigidbody.velocity.y * Time.deltaTime;
                        }
                    }
                    else
                    {
                        if (move.x >= 0)
                        {
                            distance += hitBuffer[i].rigidbody.velocity.x * Time.deltaTime;
                        }
                        else
                        {
                            distance -= hitBuffer[i].rigidbody.velocity.x * Time.deltaTime;
                        }
                    
                    }
                }
          
            }
        }
        body.position = body.position + move.normalized * distance;
    }

}