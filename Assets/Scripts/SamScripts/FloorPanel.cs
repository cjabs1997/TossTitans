using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPanel : MonoBehaviour
{

    public Activator _ac;
    // Start is called before the first frame update
    void Start()
    {
        _ac = GetComponent<Activator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
     if(other.tag == "Player")
        {
            _ac.isActive = true;
        }   
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _ac.isActive = false;
        }
    }
}
