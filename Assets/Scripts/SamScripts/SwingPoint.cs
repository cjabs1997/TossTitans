using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingPoint : MonoBehaviour
{
    public GameObject attachedPlayer;
    public GameObject joint;
    public DistanceJoint2D _dj;

    void Start()
    {
        _dj = GetComponent<DistanceJoint2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
