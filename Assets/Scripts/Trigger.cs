using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public List<Triggerable> triggerables;

    GameObject highlightParticle;
    AudioSource audioData;
    bool isTriggered = false;

    private void Start()
    {
        audioData = GetComponent<AudioSource>();
        foreach(Transform child in this.transform) 
        {
            GameObject obj = child.gameObject;
            if (obj.GetComponent<ParticleSystem>() != null)
            {
                highlightParticle = obj;
            }
        }
    }

    public bool IsTriggered()
    {
        return isTriggered;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTriggered && other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            foreach (var triggerable in triggerables)
            {
                triggerable.OnTriggered();
            }

            if (audioData != null)
            {
                audioData.Play();
            }

            if (highlightParticle != null)
            {
                highlightParticle.SetActive(false);
            }
        }
    }
}
