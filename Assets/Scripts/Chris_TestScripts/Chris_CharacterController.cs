using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_CharacterController : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("How fast the object should travel.")]
    [Range(0f, 15f)]
    [SerializeField] private float moveSpeed = 0f;

    [Range(0f, 50f)]
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
        // This is not ideal, still running a check every frame for no reason.
        // Will think of ways to prevent this.
        if (!selected)
            return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_Rigidbody2D.AddForce(Vector2.up * jumpForce);
        }
    }

    void FixedUpdate()
    {
        // This is not ideal, still running a check every fixed frame for no reason.
        // Will think of ways to prevent this.
        if (!selected)
            return;

        float direction = Input.GetAxisRaw("Horizontal");

        float jerk = moveSpeed * direction;

        jerk *= ((moveSpeed - m_Rigidbody2D.velocity.magnitude) / moveSpeed) + 100f;

        m_Rigidbody2D.AddForce(new Vector2(jerk, 0));
    }

    public void SwapSelected()
    {
        this.enabled = !this.enabled;
        selected = !selected;
        m_Rigidbody2D.simulated = !m_Rigidbody2D.simulated;
    }
}
