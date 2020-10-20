using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_GameManager : MonoBehaviour
{
    [Header("Controls")]
    [Tooltip("The key used to control when control of characters should be swapped.")]
    public KeyCode swapKey;

    [Header("Events")]
    [Tooltip("The event to raise when characters are swapped.")]
    public GameEvent swapEvent;


    private void Update()
    {
        if(Input.GetKeyDown(swapKey))
        {
            SwapCharacters();
        }
    }

    private void SwapCharacters()
    {
        if(!swapEvent)
        {
            Debug.Log("Value for 'swapEvent' has not been set yet");
            return;
        }

        swapEvent.Raise();

        // If we want some sort of swap cooldown we could put that here...
    }
}
