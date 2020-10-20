using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Sam's implementation of this via Cinemachine is much better. Used for personal testing only...
/// </summary>
public class Chris_CameraController : MonoBehaviour
{
    public Target target;
    public float yOffset;
    public List<Transform> targets;

    public enum Target
    {
        Fire = 0,
        Ice = 1
    }

    private void Update()
    {
        this.transform.position = new Vector3(targets[(int)target].position.x, targets[(int)target].position.y + yOffset, this.transform.position.z);
    }

    public void SwapTarget()
    {
        target = (Target)(((int)target + 1) % 2);
    }
}
