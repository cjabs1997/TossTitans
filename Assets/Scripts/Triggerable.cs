using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable : MonoBehaviour
{
    public CinemachineVirtualCamera followCam;

    protected bool activated;

    public virtual void OnTriggered()
    {
        activated = true;
        if(followCam != null)
        {
            followCam.LookAt = transform;
            followCam.Follow = transform;
        }
    }
}
