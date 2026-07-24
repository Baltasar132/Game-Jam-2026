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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC");
            paused = !paused;

            Time.timeScale = paused ? 0f : 1f;
            pauseText.SetActive(paused);

            Cursor.visible = paused;
            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}