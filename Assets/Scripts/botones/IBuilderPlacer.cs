using UnityEngine;

public interface IBuilderPlacer
{
    GameObject PlaceBuild(Vector3 at);
    Vector3 SnapToGrid(Vector3 at);

    bool CanPlace(Vector3 at);

    float PlaneHeight()
    {
        return 0.5f;
    }
}