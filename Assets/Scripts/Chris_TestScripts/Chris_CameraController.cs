using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if(Input.GetKeyDown(KeyCode.E))
        {
            target = (Target) (((int)target + 1) % 2);
        }
    }
}
