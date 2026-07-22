using UnityEngine;
using UnityEngine.InputSystem;

public class QButton : MonoBehaviour, IBuilderPlacer
{
    public static QButton INSTANCE;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private GameObject ghostTowerPrefab;

    void Awake()
    {
        INSTANCE = this;
    }

    void FixedUpdate()
    {
        if (InputSystem.actions["QButton"].IsPressed())
        {
            Use();
        }
    }

    void Use()
    {
        MouseHandler.CreateGhost(this.ghostTowerPrefab, this);
        // TODO: sound
    }

    GameObject IBuilderPlacer.PlaceBuild(Vector3 at)
    {
        //TODO: ResourceManager.SubRes(PriceManager.getWoodWorkerPrice());
        //TODO: ResourceManager.AddWorkers(ResType.Wood);
        //TODO: sound
        GameObject tower = Instantiate(towerPrefab, Builds.GetGameObject().transform);
        tower.transform.position = at;
        return tower;
    }
}
