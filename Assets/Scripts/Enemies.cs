using UnityEngine;

public class Enemies : MonoBehaviour
{
    public static Enemies INSTANCE;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float radius = 50f;

    void Awake()
    {
        INSTANCE = this;
    }

    void Update()
    {

    }

    public static void SpawnEnemyRandom()
    {
        GameObject newEnemy = Instantiate(INSTANCE.enemyPrefab, INSTANCE.gameObject.transform);
        float angle = Random.Range(0, 2 * Mathf.PI);
        float x = INSTANCE.radius * Mathf.Cos(angle);
        float y = INSTANCE.radius * Mathf.Sin(angle);
        newEnemy.transform.position = new(x, 0, y);
    }
}
