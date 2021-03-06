﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public InterableObject abilityInteractionState;

    private Animator m_Animator;
    private CharacterController m_CharacterController;
    private Interactable m_Interactable;
    private SpriteRenderer m_SpriteRenderer;

    private void Awake()
    {
        m_Animator = this.GetComponent<Animator>();
        m_CharacterController = this.GetComponent<CharacterController>();
        m_Interactable = this.GetComponent<Interactable>();
        m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    

    void Update()
    {
        
        if (m_CharacterController.isActive)
            // If the character is moving to the left then flip the sprite. 
            m_SpriteRenderer.flipX = m_CharacterController.velocity.x < 0;

        WalkingAnimHandler();
        InAirAnimHandler();
        JumpingAnimHandler();
        IceAnimHandler();
        DashAnimHandler();
    }

    /// <summary>
    /// Handles the animation for turning into Ice with the Interactable change.
    /// 
    /// Idea was to move Interactable change to an event but that isn't necessary with our current animation.
    /// </summary>
    private void IceAnimHandler()
    {
        // If we can't be ice ignore the things here
        // Also prevents some warning messages :)
        if (!m_CharacterController.allowIceTransform)
            return;


        // If we are an ice block turn into one
        if (m_CharacterController.GetIsIce())
        {
                m_Animator.SetBool("IceBlock", true);
                m_Interactable.SetState(abilityInteractionState);
        }
        // else we want to go back to normal
        else
        {
            m_Animator.SetBool("IceBlock", false);
            m_Interactable.ResetState();
        }

    }

    /// <summary>
    /// Handles bool setting in the animator for whether the character is walking.
    /// 
    /// Moving this to a float could be more intuitive? Not sure.
    /// </summary>
    private void WalkingAnimHandler()
    {
        // Alternatively could grab input axis but I think this looks better
        if (m_CharacterController.velocity.x != 0)
        {
            m_Animator.SetBool("Walking", true);
        }
        else
        {
            m_Animator.SetBool("Walking", false);
        }
    }

    /// <summary>
    /// Handles bool setting in the animator for whether the character is in the air.
    /// 
    /// Moving this to a float could be more intuitive? Not sure.
    /// </summary>
    private void InAirAnimHandler()
    {
        if (m_CharacterController.velocity.y != 0 || m_CharacterController.IsGrounded == false)
        {
            m_Animator.SetBool("InAir", true);
        }
        else
        {
            m_Animator.SetBool("InAir", false);
        }
    }

    /// <summary>
    /// Handles setting the trigger for if the character jumps.
    /// </summary>
    private void JumpingAnimHandler()
    {
        if (m_CharacterController.isActive && 
            ((m_CharacterController.IsGrounded && Input.GetButtonDown("Jump") && !m_CharacterController.GetIsIce()) ||
            m_CharacterController.allowDoubleJump && m_CharacterController.GetHasDoubleJump() && Input.GetButtonDown("Jump") && !m_CharacterController.GetIsIce()))
        {
            m_Animator.SetTrigger("Jump");
        }

    }

    private void DashAnimHandler()
    {
        if(!m_CharacterController.hasGrapplingHook && m_CharacterController.GetDash())
        {
            m_Animator.SetTrigger("Ability");
        }
    }
}
