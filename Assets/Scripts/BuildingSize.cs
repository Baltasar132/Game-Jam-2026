using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BuildingSize
{
    [SerializeField] public int size;

    public BuildingSize(int size)
    {
        this.size = size;
    }

    public Vector3 Snap(Vector3 vec)
    {
        return size % 2 == 0 ? Builds.NearestCross(vec) : Builds.Snap(vec);
    }

    public List<Vector3> GetBuildingPoints(Vector3 vec)
    {
        List<Vector3> list = new(size * size);
        int min = -((size - 1) / 2);
        int max = size / 2;
        for (int i = min; i <= max; i++)
        {
            for (int j = min; j <= max; j++)
            {
                list.Add(vec + new Vector3(Builds.GetCellWidth() * i, 0, Builds.GetCellWidth() * j));
            }
        }
        GetOuterPoints(vec);
        return list;
    }

    public List<Vector3> GetOuterPoints(Vector3 vec)
    {
        List<Vector3> list = new(4);
        int min = -((size - 1) / 2);
        int max = size / 2;
        // x: menor, y: menor
        list.Add(Builds.Snap(vec + new Vector3(Builds.GetCellWidth() * min, 0, Builds.GetCellWidth() * min))
                                + new Vector3(-Builds.GetCellWidth() / 2, 0, -Builds.GetCellWidth() / 2));
        // x: menor, y: mayor
        list.Add(Builds.Snap(vec + new Vector3(Builds.GetCellWidth() * min, 0, Builds.GetCellWidth() * max))
                                + new Vector3(-Builds.GetCellWidth() / 2, 0, Builds.GetCellWidth() / 2));
        // x: mayor, y: menor
        list.Add(Builds.Snap(vec + new Vector3(Builds.GetCellWidth() * max, 0, Builds.GetCellWidth() * min))
                                + new Vector3(Builds.GetCellWidth() / 2, 0, -Builds.GetCellWidth() / 2));
        // x: mayor, y: mayor
        list.Add(Builds.Snap(vec + new Vector3(Builds.GetCellWidth() * max, 0, Builds.GetCellWidth() * max))
                                + new Vector3(Builds.GetCellWidth() / 2, 0, Builds.GetCellWidth() / 2));
        return list;
    }

    public bool CanPlace(Vector3 at)
    {
        return GetBuildingPoints(at).All(Builds.CanPlaceAt);
    }
}