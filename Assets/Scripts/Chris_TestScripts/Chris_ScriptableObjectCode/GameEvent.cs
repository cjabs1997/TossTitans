// Based on the architecture described by Ryan Hipple in his Unite 2017 talk
//
//https://www.youtube.com/watch?v=raQ3iHhE_Kk
//https://github.com/roboryantron/Unite2017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Event/GameEvent")]
public class GameEvent : ScriptableObject
{
    [SerializeField]private List<GameEventListener> listeners = new List<GameEventListener>();

    /// <summary>
    /// Calls the OnEventRaised function for all currently enabled listeners.
    /// </summary>
    public void Raise()
    {
        //Debug.Log("Amount of listeners: " + listeners.Count);

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        //Debug.Log(listener.gameObject.name + " added to " + this.name);
        //if (this.name.Contains("Room"))
            //Debug.Log("--Added listener: " + listener.gameObject.name + " to " + this.name + " listener--");

        if (!listeners.Contains(listener))
            listeners.Add(listener);

        //Debug.Log("New total for " + this.name + "  " + listeners.Count);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        //Debug.Log(listener.gameObject.name + " removed from " + this.name);

        if (listeners.Contains(listener))
            listeners.Remove(listener);

        //Debug.Log("New total for " + this.name + "  " + listeners.Count);
    }
}
