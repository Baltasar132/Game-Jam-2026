using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform resourcePoint;
    public Worker.WorkerType type;

    public float speed = 5.0F;
    [SerializeField] private float workDoneDistance = 2.0F;
    public bool needsUpdate = true;
    private List<Vector3> path = new();

    [HideInInspector] public bool returning = false;

    void FixedUpdate()
    {
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
            transform.Translate(movement * speed * Time.deltaTime, Space.World);
        }

        if (needsUpdate)
        {
            Vector3 goal = GetGoal();
            Vector3 from = GetFrom();
            path = Builds.GetPath(from, goal);
            if (path.Contains(from))
            {
                path.Remove(from);
            }
            needsUpdate = false;
        }
    }

    public Vector3 GetGoal()
    {
        return returning ? spawnPoint.position : resourcePoint.position;
    }

    public Vector3 GetFrom()
    {
        return returning ? resourcePoint.position : spawnPoint.position;
    }

    public void ForcePathUpdate()
    {
        Vector3 goal = GetGoal();
        Vector3 from = this.transform.position;
        path = Builds.GetPath(from, goal);
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
