using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Builds : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellWidth = 3;

    private static Builds INSTANCE;
    public List<BuildingType> buildings;
    public List<(Vector3, int)> navPoints = new();
    public List<Vector3> buildingPositions = new();
    // (position, range)
    public List<(Vector3, float)> trees = new();
    public List<(Vector3, float)> stones = new();

    // quick methods
    public static int Width => INSTANCE.width;
    public static int Height => INSTANCE.height;
    public static float CellWidth => INSTANCE.cellWidth;
    public static List<BuildingType> Buildings => INSTANCE.buildings;
    public static List<(Vector3, float)> Trees => INSTANCE.trees;
    public static List<(Vector3, float)> Stones => INSTANCE.stones;

    void Awake()
    {
        INSTANCE = this;
        buildings = new(width * height);
        for (int i = 0; i < width * height; i++)
        {
            buildings.Add(BuildingType.None);
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

    public static Vector3 GetClosest(Vector3 from)
    {
        if (INSTANCE.buildingPositions.Count == 0)
        {
            return Vector3.zero;
        }
        Vector3 closest = INSTANCE.buildingPositions[0];
        float distance = (closest - from).sqrMagnitude;
        foreach (Vector3 pos in INSTANCE.buildingPositions)
        {
            float newDistance = (pos - from).sqrMagnitude;
            if (newDistance < distance)
            {
                closest = pos;
                distance = newDistance;
            }
        }
        return closest;
    }

    public static (Vector3?, float) GetClosestTree(Vector3 from)
    {
        if (INSTANCE.trees.Count == 0)
        {
            return (null, 0f);
        }
        (Vector3, float) closest = INSTANCE.trees[0];
        float distance = (closest.Item1 - from).sqrMagnitude;
        foreach ((Vector3, float) pos in INSTANCE.trees)
        {
            float newDistance = (pos.Item1 - from).sqrMagnitude;
            if (newDistance < distance)
            {
                closest = pos;
                distance = newDistance;
            }
        }
        return closest;
    }

    public static (Vector3?, float) GetClosestStone(Vector3 from)
    {
        if (INSTANCE.stones.Count == 0)
        {
            return (null, 0f);
        }
        (Vector3, float) closest = INSTANCE.stones[0];
        float distance = (closest.Item1 - from).sqrMagnitude;
        foreach ((Vector3, float) pos in INSTANCE.stones)
        {
            float newDistance = (pos.Item1 - from).sqrMagnitude;
            if (newDistance < distance)
            {
                closest = pos;
                distance = newDistance;
            }
        }
        return closest;
    }

    public static void AddTree(Vector3 pos, float range)
    {
        INSTANCE.trees.Add((pos, range));
    }

    public static void AddStone(Vector3 pos, float range)
    {
        INSTANCE.stones.Add((pos, range));
    }

    public static void RemoveTree(List<Vector3> occupying, BuildingSize size)
    {
        foreach (var pos in occupying)
        {
            INSTANCE.trees.RemoveAll(s => (s.Item1 - pos).sqrMagnitude < 0.001f);
        }

        Workers.OnTreeRemoved(occupying);
    }

    public static void RemoveStone(List<Vector3> occupying, BuildingSize size)
    {
        foreach (var pos in occupying)
        {
            INSTANCE.stones.RemoveAll(s => (s.Item1 - pos).sqrMagnitude < 0.001f);
        }

        Workers.OnStoneRemoved(occupying);
    }

    public static void UpdateNavPoints(List<Vector3> points)
    {
        foreach (Vector3 point in points)
        {
            bool found = false;
            int foundIdx = 0;

            foreach (var tmp in INSTANCE.navPoints)
            {
                Vector3 navPoint = tmp.Item1;
                if ((navPoint - point).magnitude < GetCellWidth() / 2)
                {
                    found = true;
                    break;
                }
                foundIdx += 1;
            }

            if (!found)
            {
                INSTANCE.navPoints.Add((point, 1));
            }
            else
            {
                INSTANCE.navPoints[foundIdx] = (INSTANCE.navPoints[foundIdx].Item1, INSTANCE.navPoints[foundIdx].Item2 + 1);
            }
        }
        Workers.UpdateWorkerPaths(points);
        Vector3 suma = Vector3.zero;
        foreach (var item in points)
        {
            suma += item;
        }
        INSTANCE.buildingPositions.Add(suma / 4);
    }

    public static List<Vector3> GetPath(Vector3 fromVec, Vector3 toVec)
    {
        float totalDistance = (fromVec - toVec).magnitude;
        var openSet = new PriorityQueue<Vector3, float>();
        var gScore = new Dictionary<Vector3, float>();
        var cameFrom = new Dictionary<Vector3, Vector3>();
        List<Vector3> points = INSTANCE.navPoints.ConvertAll((tuple) => tuple.Item1);

        points.Add(fromVec);
        points.Add(toVec);
        foreach (Vector3 node in points)
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
                return ReconstructPath(cameFrom, current);
            }

            foreach (Vector3 neighbor in GetNeighbors(points, current, GetCellWidth() * totalDistance))
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

    private static IEnumerable<Vector3> GetNeighbors(List<Vector3> list, Vector3 current, float maxRadius)
    {
        float maxRadiusSqr = maxRadius * maxRadius;
        foreach (Vector3 navPoint in list)
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

    public static void RemoveBuilding(Vector3 center, Vector3 placePoint, BuildingSize size)
    {
        // remove from building positions
        Vector3 closest = GetClosest(center);
        INSTANCE.buildingPositions.Remove(closest);

        // remove from building points
        var points = size.GetBuildingPoints(placePoint);
        foreach (var point in points)
        {
            PlaceAt(point, BuildingType.None);
        }

        // remove from navPoints (if orphan)
        var outerPoints = size.GetOuterPoints(placePoint);
        List<int> marked = new();
        List<int> markedForDel = new();
        foreach (var outer in outerPoints)
        {
            int idx = 0;
            foreach (var navPoint in INSTANCE.navPoints)
            {
                if (outer == navPoint.Item1)
                {
                    // -1 to navpoint
                    // if navpoint == 0, mark for deletion
                    marked.Add(idx);
                    if (navPoint.Item2 == 1)
                    {
                        markedForDel.Add(idx);
                    }
                    break;
                }
                idx += 1;
            }
        }
        foreach (int idx in marked)
        {
            INSTANCE.navPoints[idx] = (INSTANCE.navPoints[idx].Item1, INSTANCE.navPoints[idx].Item2 - 1);
        }

        // i hate c#
        var sortedIndices = markedForDel.OrderByDescending(i => i);
        foreach (int index in sortedIndices)
        {
            if (index >= 0 && index < INSTANCE.navPoints.Count)
            {
                INSTANCE.navPoints.RemoveAt(index);
            }
        }
    }

    public static void PrintBuildings()
    {
        string text = "";
        for (int i = 0; i < Get().height; i++)
        {
            for (int j = 0; j < Get().width; j++)
            {
                switch (GetAt(i, j))
                {
                    case BuildingType.None:
                        text += " ";
                        break;
                    case BuildingType.Center:
                        text += "C";
                        break;
                    case BuildingType.House:
                        text += "H";
                        break;
                    case BuildingType.WoodHouse:
                        text += "W";
                        break;
                    case BuildingType.StoneHouse:
                        text += "S";
                        break;
                    case BuildingType.Tower:
                        text += "T";
                        break;
                    case BuildingType.Tree:
                        text += "3";
                        break;
                    case BuildingType.Stone:
                        text += "1";
                        break;
                }
            }
            text += "\n";
        }
        print(text);
    }

    public static int StoneAmount()
    {
        return INSTANCE.stones.Count;
    }

    public static int TreeAmount()
    {
        return INSTANCE.trees.Count;
    }
}
