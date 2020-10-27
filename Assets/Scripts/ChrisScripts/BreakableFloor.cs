using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class BreakableFloor : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("If true the object will only break if hit by an appropriate object traveling the minimum velocity.")]
    public bool requiresMinSpeed;
    [Tooltip("Used if requires a minimum Speed to break. If the object is traveling at or greater than this speed then the object will break.")]
    public float minSpeedToBreak;


    private Interactable m_Interactable;

    private void Awake()
    {
        m_Interactable = this.GetComponent<Interactable>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Interactable>(out Interactable i))
        {
            // Object does not interact with the object that collided with it, ignore the rest of the interaction.
            if(!m_Interactable.GetState().interactsWith.Contains(i.GetState()) )
            {
                return;
            }

            if((requiresMinSpeed && collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude >= minSpeedToBreak) || !requiresMinSpeed)
            {
                // If we want to have some particles spawn or camera shake, we could do that here...
                Destroy(this.gameObject);
            }
        }
    }
}
