using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 targetPosition;
    private float damage;
    private float speed;
    private IHealthEnemy targetEnemy;
    private bool initialized = false;

    public void Init(Vector3 targetPos, float damage, float speed, IHealthEnemy enemy)
    {
        this.targetPosition = targetPos;
        this.damage = damage;
        this.speed = speed;
        this.targetEnemy = enemy;

        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        this.initialized = true;
    }

    void FixedUpdate()
    {
        if (!initialized) return;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (Vector3.SqrMagnitude(transform.position - targetPosition) < 0.001f)
        {
            if (targetEnemy is UnityEngine.Object unityObj && unityObj != null)
            {
                targetEnemy.DoDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}