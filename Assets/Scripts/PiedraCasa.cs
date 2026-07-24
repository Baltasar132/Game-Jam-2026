using UnityEngine;

public class PiedraCasa : MonoBehaviour, IInteractable
{
    public GameObject menu;
    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private Transform workerSpawnPoint;
    [SerializeField] private Transform workersParent;
    [SerializeField] private Transform stonePoint;

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
        if (PriceManager.getStoneLevelPrice() <= ResourceManager.GetRes())
        {
            ResourceManager.SubRes(PriceManager.getStoneLevelPrice());
            ResourceManager.AddLevel(ResType.Stone);
            Debug.Log("Stoneworks upgraded");
        }
        else
        {
            Debug.Log("Not enough money bruuuuuuh" + ResourceManager.GetRes() + " is less than " + PriceManager.getStoneLevelPrice());
        }
    }

    public void CreateWorker()
    {
        if (PriceManager.getStoneWorkerPrice() <= ResourceManager.GetRes())
        {
            ResourceManager.SubRes(PriceManager.getStoneWorkerPrice());
            ResourceManager.AddWorkers(ResType.Stone);
            GameObject new_worker = Instantiate(workerPrefab, workersParent);
            new_worker.transform.position = workerSpawnPoint.position;
            Worker new_worker2 = new_worker.GetComponent<Worker>();
            new_worker2.spawnPoint = this.workerSpawnPoint;
            new_worker2.resourcePoint = this.stonePoint;
            new_worker2.type = Worker.WorkerType.Stone;
            Workers.AddWorker(new_worker2);
        }
    }
}
