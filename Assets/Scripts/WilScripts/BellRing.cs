using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellRing : MonoBehaviour
{
    public bool isRung = false;
    AudioSource audioData;

    private void Start()
    {
        audioData = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Only allows bell to be rung one time
        if (other.gameObject.CompareTag("Player") && !isRung)
        {
            //Set isRung to true and play the bell sound here
            audioData.Play();
            isRung = true;

            //Sends message that scale listens for to denote bell has been rung
            GameObject scaleListener = GameObject.FindGameObjectWithTag("ThirdGoal");
            scaleListener.SendMessage("BellTriggered");


        }
    }
}
