using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Scene currentScene = SceneManager.GetActiveScene();

            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}