using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_GameManager : MonoBehaviour
{
    [Header("Character Settings")]
    public Character startingCharacter;
    
    // This is not a great way of handling this but it works for now :)
    public GameObject fireFella;
    public GameObject iceFella;
    public GameObject camera;


    [Header("Controls")]
    [Tooltip("The key used to control when control of characters should be swapped.")]
    public KeyCode swapKey;

    [Header("Events")]
    [Tooltip("The event to raise when characters are swapped.")]
    public GameEvent swapEvent;

    public enum Character
    {
        Fire = 0,
        Ice = 1
    }


    private void Start()
    {
        if(startingCharacter == Character.Fire)
        {
            fireFella.GetComponent<Chris_CharacterController>().SetSelected(true);
            iceFella.GetComponent<Chris_CharacterController>().SetSelected(false);
            camera.GetComponent<Chris_CameraController>().SetTarget((int)Character.Fire);
        }
        else
        {
            fireFella.GetComponent<Chris_CharacterController>().SetSelected(false);
            iceFella.GetComponent<Chris_CharacterController>().SetSelected(true);
            camera.GetComponent<Chris_CameraController>().SetTarget((int)Character.Ice);
        }
    }

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
