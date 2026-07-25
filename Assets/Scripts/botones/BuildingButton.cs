using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingButton : MonoBehaviour, IBuilderPlacer
{
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] private GameObject ghostBuildingPrefab;
    [SerializeField] private BuildingSize size = new BuildingSize(1);
    [SerializeField] private float height = 0.5f;
    [SerializeField] private string ActionName;
    [SerializeField] private BuildingType buildingType;

    void FixedUpdate()
    {
        if (InputSystem.actions[ActionName].IsPressed())
        {
            Use();
        }
    }

    void Use()
    {
        MouseHandler.CreateGhost(this.ghostBuildingPrefab, this);
        // TODO: sound
    }

    public GameObject PlaceBuild(Vector3 at)
    {
        Vector3 pos0 = SnapToGrid(at);
        List<Vector3> occuppying = size.GetBuildingPoints(at);
        //TODO: ResourceManager.SubRes(PriceManager.getWoodWorkerPrice());
        //TODO: ResourceManager.AddWorkers(ResType.Wood);
        //TODO: sound
        GameObject building = Instantiate(buildingPrefab, Builds.GetGameObject().transform);
        BuildingImpl buildingImpl = building.GetComponentInChildren<BuildingImpl>();
        buildingImpl.center = size.Center(at);
        buildingImpl.size = size;
        buildingImpl.placePoint = at;
        building.transform.position = pos0;
        foreach (var occ_pos in occuppying)
        {
            Builds.PlaceAt(occ_pos, buildingType);
        }
        Builds.NextID();
        Builds.UpdateNavPoints(size.GetOuterPoints(at));

        return building;
    }

    public Vector3 SnapToGrid(Vector3 at)
    {
        return size.Snap(at);
    }

    public bool CanPlace(Vector3 at)
    {
        return size.CanPlace(at);
    }

    public float PlaneHeight()
    {
        return height;
    }
}

