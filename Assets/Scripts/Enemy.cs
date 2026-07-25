using UnityEngine;

public class Enemy : MonoBehaviour, IHealthEnemy
{
    private readonly Collider[] hitBuffer = new Collider[16];
    public Vector3 closest = Vector3.zero;
    public float timer = 1.0f;
    public float speed = 5.0F;
    public float maxHealth = 10f;
    public float currentHealth = 10f;
    public float enemyRadius = 1.0f;
    public float buildingSearchRange = 2.0f;
    public float attackRange = 0.0f;
    public float damage = 2.0f;

    public void DoDamage(float damage)
    {
        this.currentHealth -= damage;
    }

    public float GetActionRadius()
    {
        return this.enemyRadius;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public Vector3 GetPos()
    {
        return this.transform.position;
    }

    void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            closest = Builds.GetClosest(this.transform.position);
            timer = 1.0f;
        }
        // get closest IHealth
        IHealthBuilding target = GetCloseHealth();

        if (target != null)
        {
            Vector3 movement = (target.GetPos() - transform.position).normalized;
            transform.Translate(movement * (speed * Time.fixedDeltaTime), Space.World);

            if ((target.GetPos() - transform.position).magnitude <= target.GetActionRadius() + this.enemyRadius + this.attackRange)
            {
                target.DoDamage(this.damage);
            }
        }
        else
        {
            Vector3 movement = (closest - this.transform.position).normalized;
            transform.Translate(movement * (speed * Time.fixedDeltaTime), Space.World);
        }
    }

    private IHealthBuilding GetCloseHealth()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, buildingSearchRange, hitBuffer);

        IHealthBuilding nearest = null;
        float minDistanceSqr = Mathf.Infinity;

        for (int i = 0; i < count; i++)
        {
            if (hitBuffer[i].TryGetComponent<IHealthBuilding>(out var healthComponent))
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
