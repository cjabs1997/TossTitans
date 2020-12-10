using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimationController : MonoBehaviour
{
    private Animator animator;
    private Trigger trigger;

    // Start is called before the first frame update
    void Awake()
    {
        animator = this.GetComponent<Animator>();
        trigger = this.GetComponentInParent<Trigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger.IsTriggered())
        {
            animator.SetBool("activated", true);
        }
        else
        {
            animator.SetBool("activated", false);
        }
    }
}
