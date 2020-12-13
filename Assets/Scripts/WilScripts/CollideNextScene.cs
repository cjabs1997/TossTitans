using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollideNextScene : MonoBehaviour
{
    private void Update()
    {
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);
        int charactersCount = 0;
        foreach (Collider2D collision in collisions)
        {
            if (collision.CompareTag("Player"))
            {
                charactersCount++;
            }
        }
        if (charactersCount == 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
