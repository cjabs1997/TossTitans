using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For objects that are meant to interact with things in the world. Currently will not apply to things retroactively unless the necessity arises.
/// Way of allowing interactions without the need for clogging the Physics layer matrix.
/// </summary>
public class Interactable : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The InteractableObject state that the object will begin in.")]
    public InterableObject startingState;
    [Tooltip("The default InteractableObject state. The state the object is set to if it is reset. If not set it will default to the startingState in Awake.")]
    public InterableObject defaultState;

    [Header("Debug")]
    [Tooltip("Current State of the object. Visible for Debug purposes only, do not edit!")]
    [SerializeField] private InterableObject currentState;

    private void Awake()
    {
        InitializeInteractable();
    }

    private void InitializeInteractable()
    {
        if (startingState == null)
        {
            Debug.Log("No starting state set! Make sure an InteractableObject is slotted into the starting state. Disabling the component for now...");
            this.enabled = false;
            return;
        }

        currentState = startingState;

        if (defaultState == null)
            defaultState = startingState;
    }

    public void ResetState()
    {
        currentState = defaultState;
    }

    public void SetState(InterableObject state)
    {
        currentState = state;
    }

    public InterableObject GetState()
    {
        return currentState;
    }
}