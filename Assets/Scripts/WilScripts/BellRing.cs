using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellRing : MonoBehaviour
{
    public bool isRung = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Only allows bell to be rung one time
        if (other.gameObject.CompareTag("Player") && !isRung)
        {
            //Set isRung to true and play the bell sound here
            Debug.Log("Ding!");
            isRung = true;

            //Add this to a number of bells rung list in the gamecontroller?
            //Could give this to a script inside the scale itself that checks those that have been rung by passing them all into it
            GameObject scaleListener = GameObject.FindGameObjectWithTag("ThirdGoal");
            scaleListener.SendMessage("BellTriggered");
        }
    }
}
