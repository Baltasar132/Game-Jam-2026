using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private float timer = 5.0f;
    [SerializeField] private int gold = 15;
    public float currentTimer = 5.0f;

    void Start()
    {
        ResourceManager.AddPpl(2);
        currentTimer = timer;
    }

    void FixedUpdate()
    {
        currentTimer -= Time.fixedDeltaTime;
        if (currentTimer <= 0)
        {
            ResourceManager.AddGold(gold);
            currentTimer = timer;
        }
    }
}
