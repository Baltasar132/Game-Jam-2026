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
            // if all points are far away, do not update (save on cpu)
            if (points.All((point) =>
            {
                return (worker.transform.position - point).sqrMagnitude > (worker.transform.position - worker.GetGoal()).sqrMagnitude;
            }))
            {
                return;
            }
            worker.ForcePathUpdate();
        }
    }
}
