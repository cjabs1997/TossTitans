using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Super basic, just flings a character along the given vector with the given force.
/// 
/// Currently isn't implemented fully, need to utilize the inspector to trigger it. But the logic is there, aka we just call the fling function when/where we want and it will work.
/// Also could add a slight animation if we want, could add some more character to it.
/// </summary>
public class Chris_FlingPad : MonoBehaviour
{
    [Header("Fling Pad Settings")]
    [Tooltip("How much force will be applied to the object when the pad flings.")]
    public float flingForce;
    [Tooltip("Vector at which the force will be applied. Will be normalized before applying the force.")]
    public Vector2 flingAngle;

    [Header("Debug")]
    [Tooltip("The object currently being targeted by the fling pad. Currently only flings one object at a time. Visible for debug purposes only.")]
    [SerializeField]private Rigidbody2D targetRigidBody;




    public void Fling()
    {
        if(targetRigidBody != null)
        {
            targetRigidBody.AddForce(flingAngle.normalized * flingForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        targetRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        targetRigidBody = null;
    }

    private void OnDrawGizmosSelected()
    {
        // Doesn't draw the line at the best spot but if this is something we want to use I can move it over/make it look better.
        Gizmos.DrawLine(this.transform.position, (this.transform.position + (Vector3)flingAngle*3));
    }
}
