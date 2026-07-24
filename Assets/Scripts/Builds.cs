using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Builds : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellWidth = 3;
    private int currentId = 1;

    private static Builds INSTANCE;
    public List<BuildingType> buildings;
    public List<int> ids;
    public List<Vector3> navPoints;

    void Awake()
    {
        INSTANCE = this;
        buildings = new(width * height);
        ids = new(width * height);
        for (int i = 0; i < width * height; i++)
        {
            buildings.Add(BuildingType.None);
            ids.Add(0);
        }
    }

    public static Builds Get()
    {
        return INSTANCE;
    }

    public static GameObject GetGameObject()
    {
        return INSTANCE.gameObject;
    }

    public static float GetCellWidth()
    {
        return Get().cellWidth;
    }

    public static Vector3 NearestCross(Vector3 vector)
    {
        return new Vector3(
            Mathf.Floor(vector.x / Get().cellWidth + 0.5f) * Get().cellWidth,
            0,
            Mathf.Floor(vector.z / Get().cellWidth + 0.5f) * Get().cellWidth
        );
    }

    public static Vector3 Snap(Vector3 vector)
    {
        Vector2 vec = Snap(new Vector2(vector.x, vector.z));
        return new Vector3(vec.x, 0, vec.y);
    }

    public static Vector2 Snap(Vector2 vector)
    {
        Vector2Int vec = ToCoords(vector);
        return new Vector2((vec.x - Get().width / 2) * Get().cellWidth, (vec.y - Get().height / 2) * Get().cellWidth);
    }

    public static Vector2Int ToCoords(Vector3 vector)
    {
        return ToCoords(new Vector2(vector.x, vector.z));
    }

    public static Vector2Int ToCoords(Vector2 vector)
    {
        int x = Mathf.RoundToInt(vector.x / Get().cellWidth) + Get().width / 2;
        int y = Mathf.RoundToInt(vector.y / Get().cellWidth) + Get().height / 2;
        if (x >= Get().width || y >= Get().height || x < 0 || y < 0)
        {
            Debug.Log("Trying to get coords of " + vector + " for (" + Get().width + ", " + Get().height + "), width: " + Get().cellWidth);
            if (x >= Get().width)
            {
                x = Get().width - 1;
            }
            if (x < 0)
            {
                x = 0;
            }

            if (y >= Get().height)
            {
                y = Get().height - 1;
            }
            if (y < 0)
            {
                y = 0;
            }
        }
        return new Vector2Int(x, y);
    }

    public static int GetIdAt(int x, int y)
    {
        return Get().ids[x + y * Get().width];
    }

    public static BuildingType GetAt(int x, int y)
    {
        return Get().buildings[x + y * Get().width];
    }

    public static bool CanPlaceAt(int x, int y)
    {
        return GetAt(x, y) == BuildingType.None;
    }

    public static void PlaceAt(int x, int y, BuildingType type)
    {
        Get().buildings[x + y * Get().width] = type;
        Get().ids[x + y * Get().width] = INSTANCE.currentId;
    }

    public static int GetIdAt(Vector2 vector)
    {
        Vector2Int vec = ToCoords(vector);
        return GetIdAt(vec.x, vec.y);
    }

    public static BuildingType GetAt(Vector2 vector)
    {
        Vector2Int vec = ToCoords(vector);
        return GetAt(vec.x, vec.y);
    }

    public static bool CanPlaceAt(Vector2 vector)
    {
        Vector2Int vec = ToCoords(vector);
        return CanPlaceAt(vec.x, vec.y);
    }

    public static void PlaceAt(Vector2 vector, BuildingType type)
    {
        Vector2Int vec = ToCoords(vector);
        PlaceAt(vec.x, vec.y, type);
    }

    public static int GetIdAt(Vector3 vector)
    {
        Vector2Int vec = ToCoords(vector);
        return GetIdAt(vec.x, vec.y);
    }

    public static BuildingType GetAt(Vector3 vector)
    {
        Vector2Int vec = ToCoords(vector);
        return GetAt(vec.x, vec.y);
    }

    public static bool CanPlaceAt(Vector3 vector)
    {
        Vector2Int vec = ToCoords(vector);
        return CanPlaceAt(vec.x, vec.y);
    }

    public static void PlaceAt(Vector3 vector, BuildingType type)
    {
        Vector2Int vec = ToCoords(vector);
        PlaceAt(vec.x, vec.y, type);
    }

    public static void NextID()
    {
        INSTANCE.currentId += 1;
    }

    public static void UpdateNavPoints(List<Vector3> points)
    {
        foreach (Vector3 point in points)
        {
            bool found = false;

            foreach (Vector3 navPoint in INSTANCE.navPoints)
            {
                if ((navPoint - point).magnitude < GetCellWidth() / 2)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                INSTANCE.navPoints.Add(point);
            }
        }
        Workers.UpdateWorkerPaths(points);
    }

    public static List<Vector3> GetPath(Vector3 fromVec, Vector3 toVec)
    {
        float totalDistance = (fromVec - toVec).magnitude;
        var openSet = new PriorityQueue<Vector3, float>();
        var gScore = new Dictionary<Vector3, float>();
        var cameFrom = new Dictionary<Vector3, Vector3>();

        INSTANCE.navPoints.Add(fromVec);
        INSTANCE.navPoints.Add(toVec);
        foreach (Vector3 node in INSTANCE.navPoints)
        {
            gScore[node] = float.PositiveInfinity;
        }

        gScore[fromVec] = 0f;
        openSet.Enqueue(fromVec, Vector3.Distance(fromVec, toVec));

        while (openSet.Count > 0)
        {
            Vector3 current = openSet.Dequeue();

            if (Vector3.SqrMagnitude(current - toVec) < 0.001f)
            {
                INSTANCE.navPoints.Remove(fromVec);
                INSTANCE.navPoints.Remove(toVec);
                return ReconstructPath(cameFrom, current);
            }

            foreach (Vector3 neighbor in GetNeighbors(current, GetCellWidth() * totalDistance))
            {
                float distance = Vector3.Distance(current, neighbor);
                float edgeCost = Mathf.Pow(distance, 1.5f);

                float tentativeG = gScore[current] + edgeCost;
                if (tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    float epsilon = 0.5f; // don't go direct please
                    float fScore = tentativeG + (epsilon * Vector3.Distance(neighbor, toVec));
                    openSet.Enqueue(neighbor, fScore);
                }
            }
        }

        return new();
    }

    private static IEnumerable<Vector3> GetNeighbors(Vector3 current, float maxRadius)
    {
        float maxRadiusSqr = maxRadius * maxRadius;
        foreach (Vector3 navPoint in INSTANCE.navPoints)
        {
            if (navPoint == current) { continue; }
            if ((navPoint - current).sqrMagnitude <= maxRadiusSqr)
            {
                yield return navPoint;
            }
        }
    }

    private static List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 current)
    {
        var path = new List<Vector3> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }
}
