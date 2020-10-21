using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_Freeze : MonoBehaviour
{
    [Header("Controls")]
    [Tooltip("Key to press to freeze into an iceblock.")]
    public KeyCode freezeKey;

    [Header("Freeze Settings")]
    [Tooltip("Color to change to when frozen. Mainly just used as feedback for now.")]
    public Color freezeColor;
    [Tooltip("The layer the object will be changed to. Used to allow interaction between players. Value corresponds to the index of the layer in the project layer list. Should be a value between 0 and 31.")]
    public int freezeLayer;

    [Header("Debug")]
    [Tooltip("Used to see if the object is frozen. Used for debugging purposes only.")]
    [SerializeField] private bool frozen = false;


    private SpriteRenderer m_SpriteRenderer;
    private Rigidbody2D m_Rigidbody2D;

    private Color defaultColor; // Holds the default color. After unfreezing the object's color should be set to this.
    private int defaultLayer;   // Holds the default layer. After unfreezing the object's layer should be set to this.

    private void Awake()
    {
        if(freezeLayer > 31)
        {
            Debug.Log("Freeze layer is out of bounds! Double check with the projects layer list to ensure you have an appropriate value selected. Setting to the Default layer (0) for now.");
            freezeLayer = 0;
        }

        m_Rigidbody2D = this.GetComponent<Rigidbody2D>();

        defaultLayer = this.gameObject.layer;

        m_SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        defaultColor = m_SpriteRenderer.color;
    }

    private void Update()
    {
        if(Input.GetKeyDown(freezeKey))
        {
            ToggleFreeze();
        }
    }

    private void ToggleFreeze()
    {
        if(frozen)
        {
            frozen = false;
            this.gameObject.layer = defaultLayer;
            m_SpriteRenderer.color = defaultColor;
            m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            frozen = true;
            this.gameObject.layer = freezeLayer;
            m_SpriteRenderer.color = freezeColor;
            m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
        }
    }
}
