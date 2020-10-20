using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sam_Rope : MonoBehaviour
{
    public DistanceJoint2D _dj;
    public LineRenderer _lr;
    public bool inuse;
    public Vector3 basePoint;
    // Start is called before the first frame update
    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _dj = GetComponent<DistanceJoint2D>();
        basePoint = _dj.connectedAnchor;
        _lr.SetPosition(1, _dj.connectedAnchor);
       
    }

    // Update is called once per frame
    void Update()
    {
        if (_dj.connectedBody != null)
        {
            _lr.SetPosition(1, _dj.connectedBody.position);
        }
        else
        {
            _lr.SetPosition(1, basePoint);
        }
    }
}
