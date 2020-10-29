using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public bool isActive;
    private int pressers;  //The button will work if one or both titans are on it
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        pressers = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void stepOn()  //stepOn and stepOff aim to prevent one titan stepping off disabling the button while the other is still on it
    {
        pressers++;
        if (pressers > 0)
            isActive = true;
    }
    
    public void stepOff()
    {
        pressers--;
        if (pressers == 0)
            isActive = false;
    }
}
