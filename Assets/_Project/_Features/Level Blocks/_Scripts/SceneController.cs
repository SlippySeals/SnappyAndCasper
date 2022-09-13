using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    Scene targetScene;


    public void Respawn()
    {
        targetScene = SceneManager.GetActiveScene();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(targetScene.name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);  
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("The application has been closed");
        Application.Quit();
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PrevScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void LoadSpecificLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}
