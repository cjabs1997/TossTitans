using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reveal : Triggerable
{
    private new SpriteRenderer renderer;
    private new Collider2D collider;
    private int numBellsTriggered = 0;

    public override void OnTriggered()
    {
        numBellsTriggered++;
        if (numBellsTriggered == 5)
        {         
            ShowAll(true);
            base.OnTriggered();
        }
    }

    void Awake()
    {
        ShowAll(false);
    }

    private void ShowAll(bool show)
    {
        //These are for the parent
        renderer = gameObject.GetComponent<SpriteRenderer>();
        collider = gameObject.GetComponent<Collider2D>();

        renderer.enabled = show;
        collider.enabled = show;

        //This is for all the children
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(show);
        }
    }
}
