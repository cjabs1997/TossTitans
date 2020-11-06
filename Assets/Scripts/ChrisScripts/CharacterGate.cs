using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Most of the stuff here is just a quick demo of some simple interactions and how the tools can be used.
/// Many of the gates have inherent issues that we can flesh out or make more precise but I just wanted to get this stuff out there,
/// 
/// These can be tweaked or expanded upon as needed.
/// </summary>
[RequireComponent(typeof(Interactable))]
public class CharacterGate : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Used to determine the type of gate and what it will do when it is interacted by another object. We can add to this list as needed." +
        "BlockGate: Forces characters that interact with it backwards (based on their velocity vector)." +
        "LaunchGate: Forces characters that interact with it forwards (based on their velocity vector)." +
        "KillGate: Resets characters that shouldn't come through the gate (this might be a little weird rn).")]
    public Type gateType;

    [Tooltip("The amount of force added to the object, used in BlockGates & LaunchGates.")]
    public float launchForce;
    [Tooltip("The direction which to apply the force, used in LaunchGates. Decides whether to launch left or right based on object's velocity")]
    public Vector2 forceDirection;

    [Tooltip("Where the object gets moved if the gate 'kills' them, used in KillGates. We can change the interaction here to do what we want but this is just a simple " +
        "demo of the things we could do.")]
    public Vector2 killPosition;

    public Animator ScreenAnimator;


    // This should probably be moved to a ScriptableObject in the future if these are something we are going to frequently use.
    public enum Type
    {
        BlockGate,
        LaunchGate,
        KillGate
    }

    private Interactable m_Interactable;
    private CinemachineImpulseSource m_CinemachineImpulseSource;

    private void Awake()
    {
        m_Interactable = this.GetComponent<Interactable>();
        m_CinemachineImpulseSource = this.GetComponent<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Type interactionType = gateType;

        // If the object that collided is something interactable.
        if (collision.gameObject.TryGetComponent<Interactable>(out Interactable i))
        {
            // If this object interacts with the object that collided with it.
            if (m_Interactable.GetState().interactsWith.Contains(i.GetState()))
            {
                switch (interactionType)
                {
                    case Type.BlockGate:
                        BlockGateAction(collision);
                        break;
                    case Type.LaunchGate:
                        LaunchGateAction(collision);
                        break;
                    case Type.KillGate:
                        KillGateAction(collision);
                        break;

                }
            }
        }
    }

    private void BlockGateAction(Collider2D collision)
    {
        collision.attachedRigidbody.AddForce(collision.attachedRigidbody.velocity.normalized * -launchForce, ForceMode2D.Impulse);
    }

    private void LaunchGateAction(Collider2D collision)
    {
        float leftOrRight = Vector2.Dot(collision.attachedRigidbody.velocity.normalized, Vector2.right);
        collision.attachedRigidbody.AddForce(new Vector2(forceDirection.normalized.x * leftOrRight, forceDirection.normalized.y) * launchForce, ForceMode2D.Impulse);
    }


    // This one feels the worst right now but we can easily add whatever we want here.
    // Simply just moves the character to the given position for now.
    private void KillGateAction(Collider2D collision)
    {
        m_CinemachineImpulseSource.GenerateImpulse();
        ScreenAnimator.SetTrigger("FlashRed");
        collision.attachedRigidbody.Sleep();
        collision.gameObject.transform.position = killPosition;
        collision.attachedRigidbody.WakeUp();
        collision.attachedRigidbody.velocity = Vector2.zero;
    }
}
