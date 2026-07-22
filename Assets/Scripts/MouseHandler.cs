using UnityEngine;
using UnityEngine.InputSystem;

public class MouseHandler : MonoBehaviour
{
    public static MouseHandler INSTANCE;
    // nullable
    private GameObject ghost;
    // nullable
    private IBuilderPlacer placer;

    void Awake()
    {
        INSTANCE = this;
    }

    void Update()
    {
        if (ghost != null)
        {
            Plane ground = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(InputSystem.actions["Point"].ReadValue<Vector2>());

            if (ground.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                ghost.transform.position = hitPoint;
            }
        }
    }

    void FixedUpdate()
    {
        if (ghost != null && InputSystem.actions["Click"].IsPressed())
        {
            Plane ground = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(InputSystem.actions["Point"].ReadValue<Vector2>());

            if (ground.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                placer.PlaceBuild(hitPoint);

                if (!InputSystem.actions["ShiftButton"].IsPressed())
                {
                    // continuing building
                    RemoveGhost();
                }
            }
        }
    }

    public static void CreateGhost(GameObject prefab, IBuilderPlacer placer)
    {
        RemoveGhost();
        GameObject ghost = Instantiate(prefab);
        ghost.transform.position = Vector3.zero;
        SetGhost(ghost, placer);
    }

    public static void SetGhost(GameObject ghost, IBuilderPlacer placer)
    {
        INSTANCE.ghost = ghost;
        INSTANCE.placer = placer;
    }

    public static void RemoveGhost()
    {
        Destroy(INSTANCE.ghost);
        INSTANCE.ghost = null;
        INSTANCE.placer = null;
    }

    public static bool HasGhost()
    {
        return INSTANCE.ghost != null;
    }
}