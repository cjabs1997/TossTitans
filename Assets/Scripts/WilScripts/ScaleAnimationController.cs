using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimationController : MonoBehaviour
{
    private Animator animator;
    private Activator scaleActivator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = this.GetComponent<Animator>();
        scaleActivator = this.GetComponentInParent<Activator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scaleActivator.isActive)
        {
            animator.SetBool("activated", true);
        }
        else
        {
            animator.SetBool("activated", false);
        }
    }
}
