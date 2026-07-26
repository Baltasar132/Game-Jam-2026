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

    public static void SpawnEnemyCircle()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        float x = INSTANCE.radius * Mathf.Cos(angle);
        float y = INSTANCE.radius * Mathf.Sin(angle);
        SpawnEnemy(new(x, 0, y));
    }

    public static void SpawnEnemy(Vector3 at)
    {
        GameObject newEnemy = Instantiate(INSTANCE.enemyPrefab, INSTANCE.gameObject.transform);
        newEnemy.transform.position = new(at.x, 0, at.z);
    }

    public static void SpawnEnemyLine(Vector3 from, Vector3 to)
    {
        Vector3 randomPoint = Vector3.Lerp(from, to, Random.value);
        SpawnEnemy(randomPoint);
    }

    public static void SpawnEnemyTop() => SpawnEnemyLine(new(Builds.MinX, 0, Builds.MaxZ), new(Builds.MaxX, 0, Builds.MaxZ));

    public static void SpawnEnemyTop(int enemyCount)
    {
        for (int _ = 0; _ < enemyCount; _++)
            SpawnEnemyTop();
    }

    public static void SpawnEnemyRight() => SpawnEnemyLine(new(Builds.MaxX, 0, Builds.MinZ), new(Builds.MaxX, 0, Builds.MaxZ));

    public static void SpawnEnemyRight(int enemyCount)
    {
        for (int _ = 0; _ < enemyCount; _++)
        {
            SpawnEnemyRight();
        }
    }

    public static void SpawnEnemyDown() => SpawnEnemyLine(new(Builds.MaxX, 0, Builds.MinZ), new(Builds.MinX, 0, Builds.MinZ));

    public static void SpawnEnemyDown(int enemyCount)
    {
        for (int _ = 0; _ < enemyCount; _++)
        {
            SpawnEnemyDown();
        }
    }

    public static void SpawnEnemyLeft() => SpawnEnemyLine(new(Builds.MinX, 0, Builds.MinZ), new(Builds.MinX, 0, Builds.MaxZ));

    public static void SpawnEnemyLeft(int enemyCount)
    {
        for (int _ = 0; _ < enemyCount; _++)
        {
            SpawnEnemyLeft();
        }
    }
}
