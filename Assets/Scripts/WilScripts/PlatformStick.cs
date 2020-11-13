using UnityEngine;

public class PlatformStick : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Test Before");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Test");
            collision.gameObject.transform.SetParent(gameObject.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.transform.parent = null;
    }
}
