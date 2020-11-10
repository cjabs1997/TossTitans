using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheel : MonoBehaviour
{
    public GameObject wheel;
    public float rotationspeed;
    public Transform[] carpoints;
    public GameObject[] cars;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wheel.transform.Rotate(0, 0, rotationspeed);
        for(int i = 0; i < cars.Length; i++)
        {
            cars[i].transform.position = carpoints[i].transform.position;
        }

    }
}
