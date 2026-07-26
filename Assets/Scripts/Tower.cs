using UnityEngine;

public class Tower : BuildingImpl
{
    [SerializeField] private float attackRange = 10.0f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float bulletSpeed = 20.0f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private static readonly Collider[] hitBuffer = new Collider[16];
    private float cooldownTimer = 0.0f;

    public override void FixedUpdate2()
    {
        base.FixedUpdate2();
        cooldownTimer -= Time.fixedDeltaTime;

        IHealthEnemy target = GetCloseHealth();

        if (target != null && cooldownTimer <= 0.0f)
        {
            Shoot(target);
            cooldownTimer = attackCooldown;
        }
    }

    private void Shoot(IHealthEnemy target)
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        Vector3 targetPos = target.GetPos();

        GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        if (bulletObj.TryGetComponent<Bullet>(out var bullet))
        {
            bullet.Init(targetPos, this.damage, this.bulletSpeed, target);
        }
    }

    private IHealthEnemy GetCloseHealth()
    {
        Vector3 origin = transform.parent != null ? transform.parent.position : transform.position;
        int count = Physics.OverlapSphereNonAlloc(origin, attackRange, hitBuffer, 1 << 8);

        IHealthEnemy nearest = null;
        float minDistanceSqr = Mathf.Infinity;

        for (int i = 0; i < count; i++)
        {
            if (hitBuffer[i] == null) continue;
            // enemy component is on parent of collider, i think
            Transform targetTransform = hitBuffer[i].transform.parent ?? hitBuffer[i].transform;
            if (targetTransform.TryGetComponent<IHealthEnemy>(out var healthComponent))
            {
                float sqrDist = (hitBuffer[i].transform.position - transform.position).sqrMagnitude;

                if (sqrDist < minDistanceSqr)
                {
                    minDistanceSqr = sqrDist;
                    nearest = healthComponent;
                }
            }
        }

        return nearest;
    }
}
