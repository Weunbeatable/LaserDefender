using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LD.Core;

namespace LD.Core
{
    public class Level : MonoBehaviour
    {
        [SerializeField] float _delayTimeInSeconds = 2f;



        public void Load_Game_Over()
        {
            StartCoroutine(_levelLoadDelay());

        }
        public void Load_Game_Scene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
            FindObjectOfType<GameSession>().ResetGame();
        }
        public void Load_Start_Menu()
        {
            SceneManager.LoadScene(0);

        }
        public void Continue()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex - 1);
        }
        public void QuitGame()
        {
            Application.Quit();
        }
        private IEnumerator _levelLoadDelay()
        {
            yield return new WaitForSeconds(_delayTimeInSeconds);
            SceneManager.LoadScene("Game_Over");
        }
    }
}
