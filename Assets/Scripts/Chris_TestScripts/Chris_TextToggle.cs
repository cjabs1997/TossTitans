using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just toggles the info text on and off.
/// </summary>
public class Chris_TextToggle : MonoBehaviour
{
    public KeyCode toggleKey;

    private UnityEngine.UI.Text m_Text;

    private void Awake()
    {
        m_Text = this.GetComponent<UnityEngine.UI.Text>();
    }

    void Update()
    {
        if(Input.GetKeyDown(toggleKey))
        {
            m_Text.enabled = !m_Text.enabled;
        }
    }
}
