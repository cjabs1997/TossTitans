using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_CharacterController : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("How fast the object should travel.")]
    [Range(0f, 15f)]
    [SerializeField] private float moveSpeed = 0f;

    [Range(0f, 10000f)]
    [Tooltip("How much force is applied to the object when you jump.")]
    [SerializeField] private float jumpForce = 0f;

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
        this.enabled = !this.enabled;

        // Gross way of doing this, but fine for testing. When all our mechanics/abilities are more streamlined and integrated we can slot this in better.
        this.GetComponent<Chris_Fling>().enabled = !this.GetComponent<Chris_Fling>().enabled;
        if (TryGetComponent<Chris_Freeze>(out Chris_Freeze freeze))
            freeze.enabled = !freeze.enabled;
    }

    /// <summary>
    /// Manually set selected. Used to more easily change the selected character.
    /// </summary>
    /// <param name="selected">The state you want the character to be in. true = enabled, false = disabled</param>
    public void SetSelected(bool selected)
    {
        this.enabled = selected;

        // Gross way of doing this, but fine for testing. When all our mechanics/abilities are more streamlined and integrated we can slot this in better.
        this.GetComponent<Chris_Fling>().enabled = selected;
        if (TryGetComponent<Chris_Freeze>(out Chris_Freeze freeze))
            freeze.enabled = selected;
    }
}
