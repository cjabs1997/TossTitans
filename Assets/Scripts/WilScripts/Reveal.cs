using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reveal : MonoBehaviour
{
    private new SpriteRenderer renderer;
    private new Collider2D collider;

    //Children variables
    private SpriteRenderer childRenderer;
    private Collider2D childCollider;

    private int numBellsTriggered;

    //public List<GameObject> bellList;
    //private GameObject currentBell;

    // Start is called before the first frame update
    void Awake()
    {
        HideAll();
        numBellsTriggered = 0;

        //foreach (GameObject bell in bellList)
        //{
        //    currentBell = bell.GetComponent<GameObject>();
        //}


    }

    // Update is called once per frame
    void Update()
    {
        if (numBellsTriggered == 5)
        {
            RevealAll();
        }
    }

    private void HideAll()
    {
        //These are for the parent
        renderer = gameObject.GetComponent<SpriteRenderer>();
        collider = gameObject.GetComponent<Collider2D>();

        renderer.enabled = false;
        collider.enabled = false;

        //This is for all the children
        for (int i = 0; i < this.transform.childCount; i++)
        {
            //Get the components of the current child in count i
            GameObject currentChild = this.transform.GetChild(i).gameObject;
            childRenderer = currentChild.GetComponent<SpriteRenderer>();
            //childCollider = currentChild.GetComponent<Collider2D>();

            //disable the renderer and collider so they are invisible and can't be interacted with
            childRenderer.enabled = false;
            //childCollider.enabled = false;  
        }
    }

    private void RevealAll()
    {
        //These are for the parent
        renderer = gameObject.GetComponent<SpriteRenderer>();
        collider = gameObject.GetComponent<Collider2D>();

        renderer.enabled = true;
        collider.enabled = true;

        //This is for all the children
        for (int i = 0; i < this.transform.childCount; i++)
        {
            //Get the components of the current child in count i
            GameObject currentChild = this.transform.GetChild(i).gameObject;
            childRenderer = currentChild.GetComponent<SpriteRenderer>();
            //childCollider = currentChild.GetComponent<Collider2D>();

            //disable the renderer and collider so they are invisible and can't be interacted with
            childRenderer.enabled = true;
            //childCollider.enabled = true;  
        }

    }

    public void BellTriggered()
    {
        numBellsTriggered++;
    }
}
