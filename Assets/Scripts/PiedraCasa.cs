using UnityEngine;

public class PiedraCasa : MonoBehaviour, IInteractable
{
    private GameObject menu;
    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private float reunionRadius = 2.0f;
    private Transform workersParent;

    void Start()
    {
        menu = transform.GetChild(0).gameObject;
        workersParent = Workers.INSTANCE.transform;
        menu.SetActive(false);
    }

    void IInteractable.ShowUI()
    {
        menu.SetActive(true);
    }

    void IInteractable.HideUI()
    {
        menu.SetActive(false);
    }

    void IInteractable.Interact() { }

    public void Upgrade()
    {
        if (PriceManager.getStoneLevelPrice() <= ResourceManager.GetRes())
        {
            ResourceManager.SubRes(PriceManager.getStoneLevelPrice());
            ResourceManager.AddLevel(ResType.Stone);
        }
        else
        {
            // TODO: sound
        }
    }

    public void CreateWorker()
    {
        if (PriceManager.getStoneWorkerPrice() <= ResourceManager.GetRes())
        {
            Vector3 centerOfBuilding = this.transform.parent.position + new Vector3(Builds.GetCellWidth() / 2, 0, Builds.GetCellWidth() / 2);
            (Vector3?, float) closestStone = Builds.GetClosestStone(centerOfBuilding);
            Vector3 spawningTowards = closestStone.Item1 ?? new Vector3(-1000f, 0f, -1000f);
            Vector3 spawnPoint = centerOfBuilding + (spawningTowards - centerOfBuilding).normalized * reunionRadius * Builds.GetCellWidth();

            ResourceManager.SubRes(PriceManager.getStoneWorkerPrice());
            ResourceManager.AddWorkers(ResType.Stone);
            GameObject new_worker = Instantiate(workerPrefab, workersParent);
            new_worker.transform.position = spawnPoint;
            Worker new_worker2 = new_worker.GetComponent<Worker>();
            new_worker2.returnPoint = spawnPoint;
            new_worker2.resourcePoint = closestStone.Item1;
            new_worker2.type = Worker.WorkerType.Stone;
            new_worker2.centerOfBuilding = centerOfBuilding;
            new_worker2.reunionRadius = reunionRadius;
            new_worker2.resourceRange = closestStone.Item2;
            Workers.AddWorker(new_worker2);
        }
    }
}
