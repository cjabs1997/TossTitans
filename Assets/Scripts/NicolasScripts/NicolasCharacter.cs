using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicolasCharacter : MonoBehaviour
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
            bool characterGrounded = Physics2D.OverlapCircle(groundCheck.position, NicolasController.instance.groundRadius, NicolasController.instance.whatisGround);
            if (characterGrounded)
            {
                _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, 0, 1f), 0);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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