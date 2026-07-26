using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Workers : MonoBehaviour
{
    public static Workers INSTANCE;
    private List<Worker> workers = new();

    void Awake()
    {
        INSTANCE = this;
    }

    void Update()
    {
        // Debug.Log(Workers.INSTANCE.workers.Count);
    }

    public static void AddWorker(Worker worker)
    {
        INSTANCE.workers.Add(worker);
    }

    public static List<Worker> GetWorkers()
    {
        return INSTANCE.workers;
    }

    public static void UpdateWorkerPaths(List<Vector3> points)
    {
        foreach (Worker worker in GetWorkers())
        {
            // if no goal, no update
            if (worker.GetGoal() == null)
            {
                return;
            }
            // if all points are far away, do not update (save on cpu)
            if (points.All((point) =>
            {
                return (worker.transform.position - point).sqrMagnitude > (worker.transform.position - worker.GetGoal().Value).sqrMagnitude;
            }))
            {
                return;
            }
            worker.ForcePathUpdate();
        }
    }

    public static void OnTreeRemoved(List<Vector3> wheres)
    {
        List<Worker> workers = INSTANCE.workers.Where(worker => worker.type == Worker.WorkerType.Wood).ToList();
        foreach (Worker worker in workers)
        {
            foreach (var where in wheres)
            {
                if (worker.resourcePoint.HasValue && Vector3.SqrMagnitude(worker.resourcePoint.Value - where) < 0.01f)
                {
                    worker.UpdateResourcePoint();
                    worker.needsUpdate = true;
                    break;
                }
            }
        }
    }

    public static void OnStoneRemoved(List<Vector3> wheres)
    {
        List<Worker> workers = INSTANCE.workers.Where(worker => worker.type == Worker.WorkerType.Stone).ToList();

        foreach (Worker worker in workers)
        {
            foreach (var where in wheres)
            {
                if (worker.resourcePoint.HasValue && Vector3.SqrMagnitude(worker.resourcePoint.Value - where) < 0.01f)
                {
                    worker.UpdateResourcePoint();
                    worker.needsUpdate = true;
                    break;
                }
            }
        }
    }
}
