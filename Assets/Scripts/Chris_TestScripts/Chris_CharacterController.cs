using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_CharacterController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float jumpForce = 0f;

    [Header("Misc")]
    [SerializeField] private bool selected = false;


    private Rigidbody2D m_Rigidbody2D;

    private void Awake()
    {
        m_Rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_Rigidbody2D.AddForce(Vector2.up * jumpForce);
        }
    }

    void FixedUpdate()
    {
        float direction = Input.GetAxisRaw("Horizontal");

        float jerk = moveSpeed * direction;

        jerk *= ((moveSpeed - m_Rigidbody2D.velocity.magnitude) / moveSpeed) + 100f;

        m_Rigidbody2D.AddForce(new Vector2(jerk, 0));
    }

    public void SwapSelected()
    {
        selected = !selected;
    }
}
