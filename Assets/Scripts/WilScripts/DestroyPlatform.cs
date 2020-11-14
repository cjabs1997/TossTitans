using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlatform : MonoBehaviour
{

    public void DestroyThis()
    {
        this.gameObject.SetActive(false);
    }
}
