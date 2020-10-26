using System.Collections;
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
    private float lerpValue;
    public GameObject highlightParticle;

    public Sam_Rope swing;
    public bool isSwinging;

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
        if (heldObject != null)
        {
            heldObject.transform.position = throwpoint.transform.position;
        }
        if(dashCooler >= 0)
        {
            
            dashCooler -= Time.deltaTime;
        }
        if(!isactive)
        {
            bool characterGrounded = Physics2D.OverlapCircle(groundCheck.position, Sam_Character_Controller.instance.groundRadius, Sam_Character_Controller.instance.whatisGround);
            if (characterGrounded)
            {
                _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, 0, 1f), 0);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Swingable" && !isSwinging && isactive)
        {
            hasDoubleJump = true;
            transform.position = other.GetComponent<DistanceJoint2D>().connectedAnchor;
            other.GetComponent<DistanceJoint2D>().connectedBody = _rb;
            swing = other.GetComponent<Sam_Rope>();
            other.GetComponent<Sam_Rope>().inUse = true;
            isSwinging = true;
        }

        if (other.tag == "SwingPoint" && !isSwinging && isactive)
        {
           if(other.GetComponent<SwingPoint>().attachedPlayer = null)
            {
                other.GetComponent<SwingPoint>().attachedPlayer = this.gameObject;

            }
        }
        if (other.gameObject.tag == "PlatformGroup") 
        {

            transform.SetParent(other.gameObject.transform);
            Debug.Log(transform.parent);

            _rb.velocity = other.GetComponent<Rigidbody2D>().velocity;
        }

        
            if (other.gameObject.tag == "PlatformGroup")
        {
            transform.SetParent(null);
        }
    }

}
