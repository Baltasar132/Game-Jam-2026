using UnityEngine;
using UnityEngine.InputSystem;

public class PrimerNivel : MonoBehaviour
{
    [SerializeField] private GameObject commandCenter;
    [SerializeField] private GameObject treePrefab;

    void Start()
    {
        foreach (Worker worker in Workers.INSTANCE.GetComponentsInChildren<Worker>())
        {
            Workers.AddWorker(worker);
        }

        BuildingButton.PlaceBuild(Vector3.zero, commandCenter, new(5), BuildingType.Center);
        BuildingButton.Place(new(15, 0, -15), treePrefab, new(1), BuildingType.Tree);
        BuildingButton.Place(new(18, 0, -15), treePrefab, new(1), BuildingType.Tree);
        BuildingButton.Place(new(15, 0, -18), treePrefab, new(1), BuildingType.Tree);
        BuildingButton.Place(new(18, 0, -18), treePrefab, new(1), BuildingType.Tree);
    }

    void FixedUpdate()
    {
        if (InputSystem.actions["F1Button"].IsPressed())
        {
            Enemies.SpawnEnemyRandom();
        }
    }
}
