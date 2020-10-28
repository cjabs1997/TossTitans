using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Activator _ac;
    public Transform point1;
    public Transform point2;
    public GameObject platform;
    public float speed;
    public bool movingUp;
    public Rigidbody2D _rb;
    void Start()
    {
        _rb = platform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_ac.isActive)
        {
            if (movingUp)
            {
                _rb.MovePosition(platform.transform.position + (transform.up * Time.fixedDeltaTime * speed));
            }
            if(!movingUp)
            {
                _rb.MovePosition(platform.transform.position - (transform.up * Time.fixedDeltaTime * speed));
            }
        }

        if(platform.transform.position.y >= point2.transform.position.y)
        {
            movingUp = false;
        }
        if(platform.transform.position.y <= point1.transform.position.y)
        {
            movingUp = true;
        }
    }


}
