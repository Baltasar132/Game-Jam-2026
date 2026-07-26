using UnityEngine;
using TMPro;


public class PauseGame : MonoBehaviour
{
    public GameObject pauseText;

    private bool paused = false;

    void Start()
    {
        pauseText.SetActive(false);
    }

 
    public void Pause()
    {
        pauseText.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        pauseText.SetActive(false);
        Time.timeScale = 1;
    }
}