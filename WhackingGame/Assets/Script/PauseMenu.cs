using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    // Start is called before the first frame update
    private void Start()
    {
        AudioManager.instance.PlayBGM();
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        AudioManager.instance._BGMSource.Pause();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        AudioManager.instance._BGMSource.UnPause();
        AudioManager.instance.PlaySFX("GameOver");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        AudioManager.instance.PlaySFX("GameOver");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.PlaySFX("GameOver");
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("SampleScene");
        AudioManager.instance.PlaySFX("GameOver");
    }

}
