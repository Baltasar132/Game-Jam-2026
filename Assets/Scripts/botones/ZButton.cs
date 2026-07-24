using UnityEngine;
using UnityEngine.InputSystem;

public class ZButton : MonoBehaviour, IBuilderPlacer
{
    public static ZButton INSTANCE;
    [SerializeField] private GameObject centerPrefab;
    [SerializeField] private GameObject ghostCenterPrefab;

    void Awake()
    {
        INSTANCE = this;
    }

    void FixedUpdate()
    {
        if (InputSystem.actions["ZButton"].IsPressed())
        {
            Use();
        }
    }

    void Use()
    {
        MouseHandler.CreateGhost(this.ghostCenterPrefab, this);
        // TODO: sound
    }

    public GameObject PlaceBuild(Vector3 at)
    {
        Vector3 pos0 = SnapToGrid(at);
        Vector3 pos1 = Builds.Snap(at);
        Vector3 pos2 = Builds.Snap(at + new Vector3(Builds.GetCellWidth(), 0, 0));
        Vector3 pos3 = Builds.Snap(at + new Vector3(0, 0, Builds.GetCellWidth()));
        Vector3 pos4 = Builds.Snap(at + new Vector3(Builds.GetCellWidth(), 0, Builds.GetCellWidth()));
        //TODO: ResourceManager.SubRes(PriceManager.getWoodWorkerPrice());
        //TODO: ResourceManager.AddWorkers(ResType.Wood);
        //TODO: sound
        GameObject tower = Instantiate(centerPrefab, Builds.GetGameObject().transform);
        tower.transform.position = pos0;
        Builds.PlaceAt(pos1, BuildingType.House);
        Builds.PlaceAt(pos2, BuildingType.House);
        Builds.PlaceAt(pos3, BuildingType.House);
        Builds.PlaceAt(pos4, BuildingType.House);
        Debug.Log("Ocupando: " + Builds.ToCoords(pos1));
        Debug.Log("Ocupando: " + Builds.ToCoords(pos2));
        Debug.Log("Ocupando: " + Builds.ToCoords(pos3));
        Debug.Log("Ocupando: " + Builds.ToCoords(pos4));
        return tower;
    }

    public Vector3 SnapToGrid(Vector3 at)
    {
        return Builds.NearestCross(at);// + new Vector3(1, 0, 1) * Builds.GetCellWidth() / 2;
    }

    public bool CanPlace(Vector3 at)
    {
        return Builds.CanPlaceAt(at)
        && Builds.CanPlaceAt(at + new Vector3(Builds.GetCellWidth(), 0, 0))
        && Builds.CanPlaceAt(at + new Vector3(0, 0, Builds.GetCellWidth()))
        && Builds.CanPlaceAt(at + new Vector3(Builds.GetCellWidth(), 0, Builds.GetCellWidth()));
    }

    public float PlaneHeight()
    {
        return 0f;
    }
}
