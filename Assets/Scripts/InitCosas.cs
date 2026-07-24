using UnityEngine;

public class InitCosas : MonoBehaviour
{
    void Start()
    {
        foreach (Worker worker in Workers.INSTANCE.GetComponentsInChildren<Worker>())
        {
            Workers.AddWorker(worker);
        }
    }

    void Update()
    {

    }
}
