using System;
using System.Collections.Generic;
using System.Linq;
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
    public bool needsUpdate = true;
    private List<Vector3> path = new();
    [HideInInspector] public float reunionRadius;

    [HideInInspector] public bool returning = false;
    [HideInInspector] public float resourceRange;
    [HideInInspector] public int carryingResource;

    void FixedUpdate()
    {
        if (GetGoal() == null)
        {
            // if it doesn't have an objective, it doesn't even try to move
            // philosophical, i know
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
                        carryingResource = ResourceManager.ReduceWood(this.transform.position, this.workDoneDistance + 1f);
                        break;
                    case WorkerType.Stone:
                        carryingResource = ResourceManager.ReduceStone(this.transform.position, this.workDoneDistance + 1f);
                        break;
                }
            }
            returning = !returning;
            needsUpdate = true;
            // TODO: ResourceManager.WorkerMakeSound();
        }
        else
        {
            Vector3 next = path.First();
            if ((next - this.transform.position).magnitude < (path.Count == 1 ? workDoneDistance + resourceRange : pathNodeDoneDistance))
            {
                path.RemoveAt(0);
            }
            Vector3 movement = (next - this.transform.position).normalized;
            transform.Translate(movement * speed * Time.fixedDeltaTime, Space.World);
        }

        if (needsUpdate)
        {
            Vector3? goal = GetGoal();
            Vector3? from = GetFrom();
            if (from == null || goal == null)
            {
                // tbf, goal is never null at this point, but it just feels wrong to not include it
                // and if from is null, i could use the workers position, but i'm lazy
                return;
            }
            path = Builds.GetPath(from.Value, goal.Value);
            if (path.Contains(from.Value))
            {
                path.Remove(from.Value);
            }
            needsUpdate = false;
        }
    }

    public Vector3? GetGoal()
    {
        return returning ? returnPoint : resourcePoint;
    }

    public Vector3? GetFrom()
    {
        return returning ? resourcePoint : returnPoint;
    }

    // used to update the path of a moving worker, so that it doesn't crash into a building
    public void ForcePathUpdate()
    {
        Vector3? goal = GetGoal();
        if (goal == null)
        {
            return;
        }
        Vector3 from = this.transform.position;
        path = Builds.GetPath(from, goal.Value);
        if (path.Contains(from))
        {
            path.Remove(from);
        }
    }

    public void UpdateResourcePoint()
    {
        switch (this.type)
        {
            case WorkerType.Wood:
                (Vector3?, float) closestTree = Builds.GetClosestTree(centerOfBuilding);
                Vector3 spawningTowards = closestTree.Item1 ?? new Vector3(-1000f, 0f, -1000f);
                Vector3 spawnPoint = centerOfBuilding + (spawningTowards - centerOfBuilding).normalized * reunionRadius * Builds.GetCellWidth();
                this.resourcePoint = closestTree.Item1;
                this.returnPoint = spawnPoint;
                this.resourceRange = closestTree.Item2;
                break;
            case WorkerType.Stone:
                (Vector3?, float) closestStone = Builds.GetClosestStone(centerOfBuilding);
                Vector3 spawningTowards2 = closestStone.Item1 ?? new Vector3(-1000f, 0f, -1000f);
                Vector3 spawnPoint2 = centerOfBuilding + (spawningTowards2 - centerOfBuilding).normalized * reunionRadius * Builds.GetCellWidth();
                this.resourcePoint = closestStone.Item1;
                this.returnPoint = spawnPoint2;
                this.resourceRange = closestStone.Item2;
                break;

        }
    }

    public enum WorkerType
    {
        Wood, Stone
    }
}
