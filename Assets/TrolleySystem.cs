using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TrolleySystem : MonoBehaviour
{
    public float speed = 5f;
    public GameObject Trolley;
    public Transform point1;
    public Transform point2;
    public Vector3 movetoPoint;
    // Start is called before the first frame update
    void Start()
    {
        Trolley.transform.position = point1.position;
        movetoPoint = point2.position;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        Trolley.transform.position = Vector3.MoveTowards(Trolley.transform.position, movetoPoint, step);

        if(Trolley.transform.position == movetoPoint)
        {
            swapPoint();
        }
    }

    void swapPoint()
    {
        if (movetoPoint == point1.position)
        {
            movetoPoint = point2.position;
        }
        else
        {
            movetoPoint = point1.position;
        }
    }
}
