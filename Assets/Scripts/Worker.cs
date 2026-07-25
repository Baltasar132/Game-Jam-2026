using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public Vector3? returnPoint;
    public Vector3? resourcePoint;
    public Worker.WorkerType type;

    public float speed = 5.0F;
    [SerializeField] private float workDoneDistance = 2.0F;
    public bool needsUpdate = true;
    private List<Vector3> path = new();

    [HideInInspector] public bool returning = false;

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
                        ResourceManager.AddWood(ResourceManager.GetLevel(ResType.Wood));
                        break;
                    case WorkerType.Stone:
                        ResourceManager.AddStone(ResourceManager.GetLevel(ResType.Stone));
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
            if ((next - this.transform.position).magnitude < workDoneDistance)
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

    public enum WorkerType
    {
        Wood, Stone
    }
}
