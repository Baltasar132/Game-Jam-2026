using UnityEngine;
using UnityEngine.InputSystem;

public class PrimerNivel : MonoBehaviour
{
    [Header("Configuración de Mapa")]
    [SerializeField] private Vector2Int minBounds = new Vector2Int(-50, -50);
    [SerializeField] private Vector2Int maxBounds = new Vector2Int(50, 50);

    [Header("Configuración de Ruido")]
    [SerializeField] private float treePlacementScale = 0.1f;
    [SerializeField] private float treePlacementThreshold = 0.6f;
    [SerializeField] private Vector2 treePlacementOffset = new(48, 96);
    [SerializeField] private float treeHeightScale = 0.1f;
    [SerializeField] private float treeHeightIncrease = 0.1f;

    [Header("Prefabs")]
    [SerializeField] private GameObject commandCenter;
    [SerializeField] private GameObject smallTreePrefab;
    [SerializeField] private GameObject mediumTreePrefab;
    [SerializeField] private GameObject bigTreePrefab;
    [SerializeField] private GameObject giantTreePrefab;


    void Start()
    {
        foreach (Worker worker in Workers.INSTANCE.GetComponentsInChildren<Worker>())
        {
            Workers.AddWorker(worker);
        }

        BuildingButton.PlaceBuild(Vector3.zero, commandCenter, new(5), BuildingType.Center);
        for (float x = minBounds.x; x <= maxBounds.x; x += Builds.GetCellWidth())
        {
            for (float z = minBounds.y; z <= maxBounds.y; z += Builds.GetCellWidth())
            {
                float sampleX = (x + treePlacementOffset.x) * treePlacementScale;
                float sampleZ = (z + treePlacementOffset.y) * treePlacementScale;

                float noiseValue = Mathf.PerlinNoise(sampleX, sampleZ);

                if (noiseValue > treePlacementThreshold)
                {
                    Vector3 spawnPos = new Vector3(x, 0f, z);
                    float sampleXHeight = (x + treePlacementOffset.x) * treeHeightScale;
                    float sampleZHeight = (z + treePlacementOffset.y) * treeHeightScale;
                    float heightNoise = Mathf.PerlinNoise(sampleXHeight, sampleZHeight) + treeHeightIncrease * spawnPos.magnitude;
                    BuildingSize size1 = new(1);
                    BuildingSize size2 = new(2);

                    if (spawnPos.magnitude > 100 && size2.CanPlace(spawnPos))
                    {
                        BuildingButton.Place(spawnPos, giantTreePrefab, new(2), BuildingType.Tree, false);
                    }
                    else
                    {
                        if (size1.CanPlace(spawnPos))
                        {
                            if (spawnPos.magnitude > 75)
                            {
                                BuildingButton.Place(spawnPos, bigTreePrefab, new(1), BuildingType.Tree, false);
                            }
                            else if (spawnPos.magnitude > 50)
                            {
                                BuildingButton.Place(spawnPos, mediumTreePrefab, new(1), BuildingType.Tree, false);
                            }
                            else
                            {
                                BuildingButton.Place(spawnPos, smallTreePrefab, new(1), BuildingType.Tree, false);
                            }
                        }
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (InputSystem.actions["F1Button"].IsPressed())
        {
            Enemies.SpawnEnemyRandom();
        }
    }
}
