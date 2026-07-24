using UnityEngine;
using UnityEngine.InputSystem;

public class QButton : MonoBehaviour, IBuilderPlacer
{
    public static QButton INSTANCE;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private GameObject ghostTowerPrefab;
    private BuildingSize size = new BuildingSize(1);

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

    public GameObject PlaceBuild(Vector3 at)
    {
        Vector3 at2 = SnapToGrid(at);
        //TODO: ResourceManager.SubRes(PriceManager.getWoodWorkerPrice());
        //TODO: ResourceManager.AddWorkers(ResType.Wood);
        //TODO: sound
        GameObject tower = Instantiate(towerPrefab, Builds.GetGameObject().transform);
        tower.transform.position = at2;
        Builds.PlaceAt(at2, BuildingType.Tower);
        Builds.NextID();
        Builds.UpdateNavPoints(size.GetOuterPoints(at));
        return tower;
    }

    public Vector3 SnapToGrid(Vector3 at)
    {
        return size.Snap(at);
    }

    public bool CanPlace(Vector3 at)
    {
        return size.CanPlace(at);
    }
}
