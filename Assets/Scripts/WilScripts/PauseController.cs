using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool isPaused = false;
    private GameObject whichPopUp;
    public Canvas popUpCanvas;


    // Update is called once per frame
    void Update()
    {
        //If the scene is currently paused due to a pop up, any key will resume time and close the pop up
        if (isPaused)
        {
            if (Input.anyKeyDown)
            {
                isPaused = false;
                Time.timeScale = 1f;
                whichPopUp.SetActive(false);
            }
        }
    }

    public void Pause(GameObject popUp)
    {
        //Uncomment pausing logic after testing the UI showing up
        isPaused = true;
        Time.timeScale = 0f;

        //Set the PopUp to set false equal to whichever gameobject was given by the pop up message
        whichPopUp = popUp;

        //Set its status to active. This set false above by any key
        whichPopUp.SetActive(true);
    }
}
