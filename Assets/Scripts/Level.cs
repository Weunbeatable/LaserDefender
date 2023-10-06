using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float delayTimeInSeconds = 2f;



    public void LoadGameOver()
    {
        StartCoroutine(levelLoadDelay());
      
    }
    public void LoadGameScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        FindObjectOfType<GameSession>().ResetGame();
    }
    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
      
    }
    public void Continue()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex-1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private IEnumerator levelLoadDelay()
    {
        yield return new WaitForSeconds(delayTimeInSeconds);
        SceneManager.LoadScene("Game_Over");
    }
}
