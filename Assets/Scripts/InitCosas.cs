using UnityEngine;
using UnityEngine.InputSystem;

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
        if (InputSystem.actions["F1Button"].IsPressed())
        {
            Enemies.SpawnEnemyCircle();
        }
    }
}
