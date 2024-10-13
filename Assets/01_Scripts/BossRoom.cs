using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoom : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            int aleatoryScene = Random.Range(0, 3);
            if (aleatoryScene == 0)
                SceneManager.LoadScene("Game");
            else if (aleatoryScene == 1)
                SceneManager.LoadScene("Game - scene 2");
            else
                SceneManager.LoadScene("Game - scene 3");
        }
    }
}
