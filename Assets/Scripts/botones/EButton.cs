using UnityEngine;
using UnityEngine.InputSystem;

public class EButton : MonoBehaviour, IBuilderPlacer
{
    public static EButton INSTANCE;
    [SerializeField] private GameObject housePrefab;
    [SerializeField] private GameObject ghostHousePrefab;

    void Awake()
    {
        INSTANCE = this;
    }

    void FixedUpdate()
    {
        if (InputSystem.actions["EButton"].IsPressed())
        {
            Use();
        }
    }

    void Use()
    {
        MouseHandler.CreateGhost(this.ghostHousePrefab, this);
        // TODO: sound
    }

    public GameObject PlaceBuild(Vector3 at)
    {
        Vector3 at2 = SnapToGrid(at);
        //TODO: ResourceManager.SubRes(PriceManager.getWoodWorkerPrice());
        //TODO: ResourceManager.AddWorkers(ResType.Wood);
        //TODO: sound
        GameObject tower = Instantiate(housePrefab, Builds.GetGameObject().transform);
        tower.transform.position = at2;
        Builds.PlaceAt(at2, BuildingType.House);
        return tower;
    }
    public Vector3 SnapToGrid(Vector3 at)
    {
        return Builds.Snap(at);
    }

    public bool CanPlace(Vector3 at)
    {
        return Builds.CanPlaceAt(at);
    }
}
