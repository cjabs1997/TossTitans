using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{

    private Animator m_Animator;
    private CharacterController m_CharacterController;
    private SpriteRenderer m_SpriteRenderer;

    private void Awake()
    {
        m_Animator = this.GetComponent<Animator>();
        m_CharacterController = this.GetComponent<CharacterController>();
        m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    

    void Update()
    {
        // If the character is moving to the left then flip the sprite.

        if(m_CharacterController.isActive)
            m_SpriteRenderer.flipX = Input.GetAxisRaw("Horizontal") < 0;

        // Alternatively could grab input axis but I think this looks better
        if(m_CharacterController.velocity.x != 0)
        {
            m_Animator.SetBool("Walking", true);
        }
        else
        {
            m_Animator.SetBool("Walking", false);
        }

        if(m_CharacterController.isActive && m_CharacterController.GetHasDoubleJump() && Input.GetButtonDown("Jump"))
        {
            m_Animator.SetTrigger("Jump");
        }

        if(m_CharacterController.velocity.y != 0 || m_CharacterController.IsGrounded == false)
        {
            m_Animator.SetBool("InAir", true);
        }
        else
        {
            m_Animator.SetBool("InAir", false);
        }
    }
}
