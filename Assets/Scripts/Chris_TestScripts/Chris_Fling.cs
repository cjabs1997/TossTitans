using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flings your current character towards the other one!
/// 
/// Will need to adjust if we actually want to implement this but just wanted to test out how this felt.
/// </summary>
public class Chris_Fling : MonoBehaviour
{
    [Header("Controls")]
    [Tooltip("Key to hold to begin charging up a fling.")]
    public KeyCode flingKey;

    [Header("Stats")]
    [Range(0f, 10000f)]
    [Tooltip("The maximum amount of force that can be applied to an object when charging a fling.")]
    public float maxMagnitude;
    [Range(0f, 5f)]
    [Tooltip("Time required to reach maximum fling force.")]
    public float timeToMax;

    [Header("Misc")]
    [Tooltip("Used to figure out the vector with which to apply the force.")]
    public Transform otherPlayer;

    private Rigidbody2D m_Rigidbody2D;

    // Used to see how charged a fling is. Probably a better way to do this...
    private float magnitudeCharged = 0f;
    private float timeCharged = 0f;

    private void Awake()
    {
        m_Rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // On the frame the key is pressed down start charging the fling.
        if(Input.GetKeyDown(flingKey))
        {
            timeCharged = 0f;
            magnitudeCharged = 0f;

            StartCoroutine(FlingRoutine());
        }

        // On the frame the key is released add force for the fling.
        else if(Input.GetKeyUp(flingKey))
        {
            
            magnitudeCharged = Mathf.Min(maxMagnitude, (timeCharged / timeToMax) * maxMagnitude); // Ensure we don't go over our maxMagnitude for fling force.
            Vector2 dirVector = (otherPlayer.position - this.transform.position).normalized;

            m_Rigidbody2D.AddForce(magnitudeCharged * dirVector, ForceMode2D.Impulse);
        }
    }

    // This is a pretty silly implementation but works for now.
    // Might be easier/better to do using input axes but don't want to mess with them to avoid any conflicts
    IEnumerator FlingRoutine()
    {
        while(timeCharged < timeToMax)
        {
            timeCharged += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}
