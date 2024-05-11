using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject pauseObj;
    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private GameObject choiceOptionObj;
    [SerializeField] private GameObject iventarioObj;
    private bool isPaused;
    public static ScenesController instance;

    public void Start()
    {
        instance = this;
    }
    public void GameOver()
    {
        gameOverObj.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1); 
        Time.timeScale = 1f;
    }
    public void PauseGame()
    {
        isPaused = !isPaused;
        pauseObj.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void OndPassed()
    {
        isPaused = !isPaused;
        choiceOptionObj.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Iventario()
    {
        isPaused = !isPaused;
        iventarioObj.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }
}
