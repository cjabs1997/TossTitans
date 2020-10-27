using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class BigBlueIce : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The InteractableObject type the object will get while in the form. Allows for different interactions with different objects while in this state.")]
    public InterableObject iceInteractionState;
    [Tooltip("Additional force that is added to the object after entering this form. For if we want an initial burst of speed when entering the form.")]
    [Range(0f, 5000f)]
    public float additionalForce;
    [Tooltip("The key to activate/deactivate the move. Will shift over to a button in the input manager in the future.")]
    public KeyCode abilityKey;

    private Animator m_Animator;
    private Interactable m_Interactable;
    private Sam_Character m_SamCharacter;
    private Rigidbody2D m_Rigidbody2D;

    private void Awake()
    {
        m_Animator = this.GetComponent<Animator>();
        m_Interactable = this.GetComponent<Interactable>();
        m_SamCharacter = this.GetComponent<Sam_Character>();
        m_Rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Character is not the active one, ignore any commands. Might also need to add additional checks here for other conditions like swinging or grounded.
        // Moving this to something that disables this script in the future so we can stop running update checks would be preferred.
        if(!m_SamCharacter.isactive)
        {
            return;
        }

        if(Input.GetKeyDown(abilityKey))
        {
            PerformAction();
        }

    }

    /// <summary>
    /// Sets the current Interactable state to the InteractableObject for the ability. 
    /// 
    /// Is called by the animator to ensure that the state change lines up with the animation.
    /// </summary>
    public void SetInteraction()
    {
        m_Interactable.SetState(iceInteractionState);
    }

    /// <summary>
    /// Resets the Interaction state of the character to its default InteractableObject.
    /// 
    /// Is called by the animator to ensure that the state change lines up with the animation.
    /// </summary>
    public void ResetState()
    {
        m_Interactable.ResetState();
    }

    /// <summary>
    /// Resimulates the rigidbody and adds the additional force if applicable.
    /// 
    /// Is called by the animator to ensure that the state change lines up with the animation.
    /// </summary>
    public void AddForce()
    {
        // This should be fine? Returns the object to the physics simulation when its done transforming.
        // If things change this can be altered to just set the gravity scale back to its default state.
        m_Rigidbody2D.simulated = true;

        if (additionalForce > 0)
        {
            m_Rigidbody2D.AddForce(Vector2.down * additionalForce, ForceMode2D.Impulse);
        }
    }

    private void PerformAction()
    {
        // If we are currently an ice block we want to return to normal
        if(m_Interactable.GetState() == iceInteractionState)
        {
            ReturnFromIce();
        }
        // else we want to become an ice block
        else
        {
            TurnIntoIce();
        }
    }

    /// <summary>
    /// Preps the rigidbody for transformation and sets the correct bool in the animator to begin the transform animation.
    /// </summary>
    private void TurnIntoIce()
    {
        // This should be fine? Locks the object out of the physics simulation while it is transforming.
        // If things change this can be altered to just set the gravity scale 0.
        m_Rigidbody2D.simulated = false;
        m_Animator.SetBool("IceBlock", true);
    }

    /// <summary>
    /// Sets the bool in the animator so the object nows to return back to its idle animation.
    /// </summary>
    private void ReturnFromIce()
    {
        m_Animator.SetBool("IceBlock", false);
    }
}
