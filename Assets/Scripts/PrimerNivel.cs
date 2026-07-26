using UnityEngine;
using UnityEngine.InputSystem;

public class PrimerNivel : MonoBehaviour
{
    [Header("Configuración de Mapa")]
    [SerializeField] private Vector2Int minBounds = new Vector2Int(-50, -50);
    [SerializeField] private Vector2Int maxBounds = new Vector2Int(50, 50);

    [Header("Configuración de Ruido, Árbol")]
    [SerializeField] private float treePlacementScale = 0.1f;
    [SerializeField, Range(0f, 1f)] private float treePlacementThreshold = 0.6f;
    [SerializeField] private Vector2 treePlacementOffset = new(48, 96);

    [Header("Configuración de Ruido, Piedra")]
    [SerializeField] private float stonePlacementScale = 1f;
    [SerializeField, Range(0f, 1f)] private float smallStonePlacementThreshold = 0.8f;
    [SerializeField, Range(0f, 1f)] private float bigStonePlacementThreshold = 0.9f;
    [SerializeField] private Vector2 stonePlacementOffset = new(102, 54);

    [Header("Prefabs")]
    [SerializeField] private GameObject commandCenter;
    [SerializeField] private GameObject smallTreePrefab;
    [SerializeField] private GameObject mediumTreePrefab;
    [SerializeField] private GameObject bigTreePrefab;
    [SerializeField] private GameObject giantTreePrefab;
    [SerializeField] private GameObject smallStonePrefab;
    [SerializeField] private GameObject bigStonePrefab;


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
                float treeNoiseX = (x + treePlacementOffset.x) * treePlacementScale;
                float treeNoiseZ = (z + treePlacementOffset.y) * treePlacementScale;
                float stoneNoiseX = (x + stonePlacementOffset.x) * stonePlacementScale;
                float stoneNoiseZ = (z + stonePlacementOffset.y) * stonePlacementScale;

                float treeNoise = Mathf.PerlinNoise(treeNoiseX, treeNoiseZ);
                float stoneNoise = Mathf.PerlinNoise(stoneNoiseX, stoneNoiseZ);
                BuildingSize size1 = new(1);
                BuildingSize size2 = new(2);
                BuildingSize size3 = new(3);
                Vector3 spawnPos = new Vector3(x, 0f, z);

                if (stoneNoise > smallStonePlacementThreshold)
                {
                    if (stoneNoise > bigStonePlacementThreshold && size3.CanPlace(spawnPos))
                    {
                        BuildingButton.Place(spawnPos, bigStonePrefab, new(3), BuildingType.Stone, false);
                    }
                    else if (size2.CanPlace(spawnPos))
                    {
                        BuildingButton.Place(spawnPos, smallStonePrefab, new(2), BuildingType.Stone, false);
                    }
                }

                if (treeNoise > treePlacementThreshold)
                {

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
