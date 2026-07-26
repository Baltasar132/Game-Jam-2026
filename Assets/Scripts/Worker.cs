using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public Vector3 centerOfBuilding;
    public Vector3? returnPoint;
    public Vector3? resourcePoint;
    public Worker.WorkerType type;

    public float speed = 5.0F;
    [SerializeField] private float workDoneDistance = 1.0F;
    [SerializeField] private float pathNodeDoneDistance = 0.2F;
    public bool needsPathUpdate = true;
    private List<Vector3> path = new();
    [HideInInspector] public float reunionRadius;

    [HideInInspector] public bool returning = false;
    [HideInInspector] public float resourceRange;
    [HideInInspector] public int carryingResource;
    private List<Vector3> cachedPathToResource; // returnPoint -> resourcePoint (no start point)
    private List<Vector3> cachedPathToReturn;   // resourcePoint -> returnPoint (no start point)
    private Vector3? cachedResourcePoint;
    private Vector3? cachedReturnPoint;

    void FixedUpdate()
    {
        if (GetGoal() == null)
        {
            // if it doesn't have an objective, it doesn't even try to move
            // philosophical, i know
            return;
        }

        bool stayPut = path?.Count == 0 && cachedPathToResource?.Count == 0 && cachedPathToReturn?.Count == 0;

        if (stayPut)
        {
            return;
        }

        if (path.Count == 0)
        {
            if (returning)
            {
                switch (type)
                {
                    case WorkerType.Wood:
                        ResourceManager.AddWood(carryingResource);
                        break;
                    case WorkerType.Stone:
                        ResourceManager.AddStone(carryingResource);
                        break;
                }
                carryingResource = 0;
            }
            else
            {
                switch (type)
                {
                    case WorkerType.Wood:
                        carryingResource = ResourceManager.ReduceWood(this.transform.position, this.workDoneDistance + resourceRange);
                        break;
                    case WorkerType.Stone:
                        carryingResource = ResourceManager.ReduceStone(this.transform.position, this.workDoneDistance + resourceRange);
                        break;
                }

            }
            returning = !returning;
            needsPathUpdate = true;
            // TODO: ResourceManager.WorkerMakeSound();
        }
        else
        {
            Vector3 next = path[0];
            if ((next - this.transform.position).magnitude < (path.Count == 1 ? workDoneDistance + resourceRange : pathNodeDoneDistance))
            {
                path.RemoveAt(0);
            }
            Vector3 movement = (next - this.transform.position).normalized;
            transform.Translate(movement * speed * Time.fixedDeltaTime, Space.World);
        }

        if (needsPathUpdate)
        {
            UpdatePath(GetGoal(), GetFrom());
            needsPathUpdate = false;
        }
    }

    public Vector3? GetGoal()
    {
        if (resourcePoint == null)
            return returnPoint;

        return returning ? returnPoint : resourcePoint;
    }
    public Vector3? GetFrom() => returning ? resourcePoint : returnPoint;

    // used to update the path of a moving worker, so that it doesn't crash into a building
    public void ForcePathUpdate()
    {
        InvalidateCache();
        UpdatePath(GetGoal(), this.transform.position);
    }

    public void UpdateResourcePoint()
    {
        Vector3? oldResource = resourcePoint;
        Vector3? oldReturn = returnPoint;

        switch (this.type)
        {
            case WorkerType.Wood:

                (Vector3? closest1, float range1) = Builds.GetClosestTree(centerOfBuilding);
                resourcePoint = closest1;
                resourceRange = range1;
                break;
            case WorkerType.Stone:
                (Vector3? closest2, float range2) = Builds.GetClosestStone(centerOfBuilding);
                resourcePoint = closest2;
                resourceRange = range2;
                break;
        }
        Vector3 target = resourcePoint ?? new Vector3(-1000f, 0f, -1000f);
        returnPoint = centerOfBuilding + (target - centerOfBuilding).normalized * reunionRadius * Builds.GetCellWidth();

        if (PointsEqual(oldResource, resourcePoint) && PointsEqual(oldReturn, returnPoint))
            return;

        InvalidateCache();
        UpdatePath(GetGoal(), transform.position);
    }

    public void UpdatePath(Vector3? goal, Vector3? from)
    {
        if (from == null || goal == null)
        {
            return;
        }

        bool isStandardRoute = resourcePoint.HasValue && returnPoint.HasValue && (
                    (PointsEqual(from, returnPoint) && PointsEqual(goal, resourcePoint)) ||
                    (PointsEqual(from, resourcePoint) && PointsEqual(goal, returnPoint))
                );

        if (isStandardRoute && cachedResourcePoint.HasValue && cachedReturnPoint.HasValue &&
                    PointsEqual(cachedResourcePoint, resourcePoint) &&
                    PointsEqual(cachedReturnPoint, returnPoint))
        {
            bool goingToResource = PointsEqual(goal, resourcePoint);

            if (goingToResource && cachedPathToResource != null)
            {
                path = new List<Vector3>(cachedPathToResource);
                return;
            }
            if (!goingToResource && cachedPathToReturn != null)
            {
                path = new List<Vector3>(cachedPathToReturn);
                return;
            }
        }
        path = Builds.GetPath(from.Value, goal.Value, PriceManager.WorkerRange);
        if (path.Contains(from.Value))
            path.Remove(from.Value);

        if (isStandardRoute)
        {
            cachedResourcePoint = resourcePoint;
            cachedReturnPoint = returnPoint;

            bool goingToResource = PointsEqual(goal, resourcePoint);
            if (goingToResource)
                cachedPathToResource = new List<Vector3>(path);
            else
                cachedPathToReturn = new List<Vector3>(path);
        }
    }


    private void InvalidateCache()
    {
        cachedResourcePoint = null;
        cachedReturnPoint = null;
        cachedPathToResource = null;
        cachedPathToReturn = null;
    }

    private static bool PointsEqual(Vector3? a, Vector3? b)
    {
        if (!a.HasValue || !b.HasValue) return false;
        return (a.Value - b.Value).sqrMagnitude < 0.01f;
    }

    public enum WorkerType
    {
        Wood, Stone
    }
}
