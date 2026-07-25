using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public GameObject AnimationHolder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()

    {
        StartCoroutine(StartGameSeq());
        AnimationHolder.SetActive(true);
        
    }

    

    // Update is called once per frame
    public void QuitGame()
    {
        Debug.Log("saliendo");
        Application.Quit();
    }

    IEnumerator StartGameSeq()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
