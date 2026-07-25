using UnityEngine;

public class PiedraCasa : MonoBehaviour, IInteractable
{
    public GameObject menu;
    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private float reunionRadius = 2.0f;
    private Transform workersParent;

    void Start()
    {
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
            Vector3? closestStone = Builds.GetClosest(this.transform.position, BuildingType.Stone);
            Vector3 spawnPoint = closestStone.HasValue
                ? this.transform.parent.position + (this.transform.parent.position - closestStone.Value).normalized * reunionRadius
                : new Vector3(-1000f, 0f, -1000f);
            ;

            ResourceManager.SubRes(PriceManager.getStoneWorkerPrice());
            ResourceManager.AddWorkers(ResType.Stone);
            GameObject new_worker = Instantiate(workerPrefab, workersParent);
            new_worker.transform.position = spawnPoint;
            Worker new_worker2 = new_worker.GetComponent<Worker>();
            new_worker2.returnPoint = spawnPoint;
            new_worker2.resourcePoint = closestStone;
            new_worker2.type = Worker.WorkerType.Stone;
            Workers.AddWorker(new_worker2);
        }
    }
}
