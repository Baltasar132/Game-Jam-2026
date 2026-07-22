using UnityEngine;

public class Worker : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform resourcePoint;
    public Worker.WorkerType type;

    public float speed = 5.0F;

    [HideInInspector] public bool returning = false;

    void FixedUpdate()
    {
        Vector3 goal = returning ? spawnPoint.position : resourcePoint.position;
        Vector3 movement = (goal - this.transform.position).normalized;

        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        if ((goal - this.transform.position).magnitude < 2.0F)
        {
            if (returning)
            {
                switch (type)
                {
                    case WorkerType.Wood:
                        ResourceManager.AddWood(ResourceManager.GetLevel(ResType.Wood));
                        break;
                    case WorkerType.Stone:
                        ResourceManager.AddStone(ResourceManager.GetLevel(ResType.Stone));
                        break;
                }
            }
            returning = !returning;
            // ResourceManager.WorkerMakeSound();
        }
    }

    public enum WorkerType
    {
        Wood, Stone
    }
}
