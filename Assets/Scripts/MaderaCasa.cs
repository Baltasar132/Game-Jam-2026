using UnityEngine;

public class MaderaCasa : MonoBehaviour, IInteractable
{
    public GameObject menu;
    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private Transform workerSpawnPoint;
    [SerializeField] private Transform workersParent;
    [SerializeField] private Transform treePoint;

    void Start()
    {
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
            Debug.Log("Not enough money bruuuuuuh" + ResourceManager.GetRes() + " is less than " + PriceManager.getWoodLevelPrice());
        }
    }

    public void CreateWorker()
    {
        if (PriceManager.getWoodWorkerPrice() <= ResourceManager.GetRes())
        {
            ResourceManager.SubRes(PriceManager.getWoodWorkerPrice());
            ResourceManager.AddWorkers(ResType.Wood);
            GameObject new_worker = Instantiate(workerPrefab, workersParent);
            new_worker.transform.position = workerSpawnPoint.position;
            Worker new_worker2 = new_worker.GetComponent<Worker>();
            new_worker2.spawnPoint = this.workerSpawnPoint;
            new_worker2.resourcePoint = this.treePoint;
            new_worker2.type = Worker.WorkerType.Wood;
        }
    }
}
