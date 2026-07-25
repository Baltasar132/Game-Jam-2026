using UnityEngine;

public class MaderaCasa : MonoBehaviour, IInteractable
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
        if (PriceManager.getWoodLevelPrice() <= ResourceManager.GetRes())
        {
            ResourceManager.SubRes(PriceManager.getWoodLevelPrice());
            ResourceManager.AddLevel(ResType.Wood);
        }
        else
        {
            // TODO: sound
        }
    }

    public void CreateWorker()
    {
        if (PriceManager.getWoodWorkerPrice() <= ResourceManager.GetRes())
        {
            Vector3 centerOfBuilding = this.transform.parent.position + new Vector3(Builds.GetCellWidth(), 0, Builds.GetCellWidth());
            Vector3? closestTree = Builds.GetClosestTree(centerOfBuilding);
            Vector3 spawningTowards = closestTree ?? new Vector3(-1000f, 0f, -1000f); // the fuck is ?? ?????? (vscode quickfix)
            Vector3 spawnPoint = centerOfBuilding + (spawningTowards - centerOfBuilding).normalized * reunionRadius * Builds.GetCellWidth();

            ResourceManager.SubRes(PriceManager.getWoodWorkerPrice());
            ResourceManager.AddWorkers(ResType.Wood);
            GameObject new_worker = Instantiate(workerPrefab, workersParent);
            new_worker.transform.position = spawnPoint;
            Worker new_worker2 = new_worker.GetComponent<Worker>();
            new_worker2.returnPoint = spawnPoint;
            new_worker2.resourcePoint = closestTree;
            new_worker2.type = Worker.WorkerType.Wood;
            new_worker2.centerOfBuilding = centerOfBuilding;
            new_worker2.reunionRadius = reunionRadius;
            Workers.AddWorker(new_worker2);
        }
    }
}
