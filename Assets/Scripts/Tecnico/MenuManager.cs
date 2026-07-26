using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public GameObject AnimationHolder;

    [Header("Nombre de la escena de destino a cargar")]
    [SerializeField] private string targetSceneName;

    public void StartGame()

    {
        StartCoroutine(StartGameSeq());
        AnimationHolder.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("saliendo");
        Application.Quit();
    }

    IEnumerator StartGameSeq()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(targetSceneName);
    }
}
