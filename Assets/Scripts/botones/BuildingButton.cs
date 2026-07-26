using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.fontSize = 18;
    }

    void FixedUpdate()
    {
        if (InputSystem.actions[ActionName].IsPressed())
        {
            Use();
        }
        text.SetText(ActionName + "\n" + buildingType + "\n" + PriceManager.GetBuildingCost(buildingType).ToStringTMP());
    }

    public BuildingType GetBuildingType()
    {
        return buildingType;
    }

    void Use()
    {
        MouseHandler.CreateGhost(this.ghostBuildingPrefab, this);
        // TODO: sound
    }

    public void PlaceBuild(Vector3 at)
    {
        //TODO: sound
        ResourceManager.SubRes(PriceManager.GetBuildingCost(buildingType));
        PriceManager.AddBuilding(buildingType);
        PlaceBuild(at, buildingPrefab, size, buildingType);
    }

    public static void Place(Vector3 at, GameObject build, BuildingSize size, BuildingType type, bool randomRotation)
    {
        Vector3 pos0 = size.Snap(at);
        List<Vector3> occupying = size.GetBuildingPoints(at);
        GameObject building = Instantiate(build, Builds.GetGameObject().transform);
        building.transform.position = pos0;

        WoodSource woodSource = building.GetComponentInChildren<WoodSource>();
        StoneSource stoneSource = building.GetComponentInChildren<StoneSource>();
        if (stoneSource != null)
        {
            stoneSource.occupying = occupying;
        }
        else if (woodSource != null)
        {
            woodSource.occupying = occupying;
        }
        else
        {
            print("StoneSource nor WoodSource found for: " + type);
        }

        if (randomRotation)
        {
            building.transform.Rotate(Vector3.up, Random.Range(0, 360));
        }

        foreach (var occ_pos in occupying)
        {
            Builds.PlaceAt(occ_pos, type);
            switch (type)
            {
                case BuildingType.Tree:
                    Builds.AddTree(occ_pos, size.size * Builds.GetCellWidth() / 2f);
                    break;
                case BuildingType.Stone:
                    Builds.AddStone(occ_pos, size.size * Builds.GetCellWidth() / 2f);
                    break;
            }
        }
    }

    public static void PlaceBuild(Vector3 at, GameObject build, BuildingSize size, BuildingType type)
    {
        Vector3 pos0 = size.Snap(at);
        List<Vector3> occuppying = size.GetBuildingPoints(at);
        GameObject building = Instantiate(build, Builds.GetGameObject().transform);
        BuildingImpl buildingImpl = building.GetComponentInChildren<BuildingImpl>();
        buildingImpl.center = size.Center(at);
        buildingImpl.size = size;
        buildingImpl.placePoint = at;
        building.transform.position = pos0;
        foreach (var occ_pos in occuppying)
        {
            Builds.PlaceAt(occ_pos, type);
        }
        Builds.UpdateNavPoints(size.GetOuterPoints(at));
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

