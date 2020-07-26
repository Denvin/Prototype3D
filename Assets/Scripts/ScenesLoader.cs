using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{
    [SerializeField] float loadTime = 1f;
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void RestartScene()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(WaitScene(activeSceneIndex));
    }

    public void LoadNextLevel()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(WaitScene(activeSceneIndex + 1));
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator WaitScene(int sceneIndex)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneIndex);
    }
}
