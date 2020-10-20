using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sam_Rope : MonoBehaviour
{
    public LineRenderer _lr;
    public DistanceJoint2D _dj;
    public Vector2 basePoint;
    public bool inUse;
    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _dj = GetComponent<DistanceJoint2D>();
        basePoint = _dj.connectedAnchor;
        _lr.SetPosition(1, basePoint);
    }

    // Update is called once per frame
    void Update()
    {
        if (inUse)
        {
            _lr.SetPosition(1, _dj.connectedBody.transform.position);
        }
        else
        {
            _lr.SetPosition(1, basePoint);
            _dj.connectedAnchor = basePoint;
        }
    }
}
