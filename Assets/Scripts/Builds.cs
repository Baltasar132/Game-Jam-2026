using System.Collections.Generic;
using UnityEngine;

public class Builds : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellWidth = 3;

    private static Builds INSTANCE;
    public List<BuildingType> buildings;

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
            Mathf.Round(vector.x / Get().cellWidth) * Get().cellWidth,
            0,
            Mathf.Round(vector.z / Get().cellWidth) * Get().cellWidth
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
}
