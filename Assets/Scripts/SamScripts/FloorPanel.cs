using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPanel : MonoBehaviour
{

    public Activator _ac;
    private Interactable m_Interactable;
    // Start is called before the first frame update
    void Start()
    {
        _ac = GetComponent<Activator>();
        m_Interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the object that collided is something interactable.
        if (other.gameObject.TryGetComponent<Interactable>(out Interactable i))
            // If this object interacts with the object that collided with it.
            if (m_Interactable.GetState().interactsWith.Contains(i.GetState()))
                _ac.stepOn();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If the object that collided is something interactable.
        if (other.gameObject.TryGetComponent<Interactable>(out Interactable i))
            // If this object interacts with the object that collided with it.
            if (m_Interactable.GetState().interactsWith.Contains(i.GetState()))
                _ac.stepOff();
    }
}
