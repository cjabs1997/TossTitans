using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePickup : MonoBehaviour
{
    public GameObject pauseController;
    public GameObject whichPopUp;

    //Entering hitbox of object causes a pause and the given UI object to show up
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pauseController.SendMessage("Pause", whichPopUp);
            Destroy(this.gameObject);
        }
    }
}
