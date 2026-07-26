using UnityEngine;

public interface IBuilderPlacer
{
    void PlaceBuild(Vector3 at);
    Vector3 SnapToGrid(Vector3 at);

    bool CanPlace(Vector3 at);

    BuildingType GetBuildingType();

    float PlaneHeight()
    {
        return 0.5f;
    }
}