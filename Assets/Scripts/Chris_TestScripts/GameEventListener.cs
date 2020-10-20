// Based on the architecture described by Ryan Hipple in his Unite 2017 talk
//
//https://www.youtube.com/watch?v=raQ3iHhE_Kk
//https://github.com/roboryantron/Unite2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
