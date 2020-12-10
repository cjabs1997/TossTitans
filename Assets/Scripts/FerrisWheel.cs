using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheel : Triggerable
{
    public CinemachineVirtualCamera followCam;
    public GameObject wheel;
    public float rotationspeed;
    public Transform[] carpoints;
    public GameObject[] cars;

    float timer = -0.2f;
    bool activated;

    public override void OnTriggered()
    {
        followCam.LookAt = transform;
        followCam.Follow = transform;
        activated = true;
    }

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
