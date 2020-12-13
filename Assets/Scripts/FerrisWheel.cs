using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheel : Triggerable
{
    public GameObject wheel;
    public float rotationspeed;
    public Transform[] carpoints;
    public GameObject[] cars;

    float timer = -0.2f;

    void Update()
    {
        if (activated)
        {
            timer += Time.deltaTime;
            wheel.transform.Rotate(0, 0, Mathf.Min(timer, 2) * rotationspeed);
            for(int i = 0; i < cars.Length; i++)
            {
                cars[i].transform.position = carpoints[i].transform.position;
            }
        }
    }
}
