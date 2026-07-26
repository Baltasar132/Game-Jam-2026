using UnityEngine;

public class Enemy : MonoBehaviour, IHealthEnemy
{
    private readonly Collider[] hitBuffer = new Collider[16];
    public Vector3 closest = Vector3.zero;
    public float timer = 1.0f;
    public float attackTimer = 1.0f;
    public float speed = 5.0F;
    public float maxHealth = 10f;
    public float currentHealth = 10f;
    public float enemyRadius = 1.0f;
    public float buildingSearchRange = 2.0f;
    public float attackRange = 0.0f;
    public float damage = 2.0f;

    [SerializeField] private float knockbackForce = 5.0f;
    [SerializeField] private float knockbackDecay = 15.0f;
    private Vector3 knockbackVelocity = Vector3.zero;

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
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }

        if (knockbackVelocity.sqrMagnitude > 0.001f)
        {
            transform.Translate(knockbackVelocity * Time.fixedDeltaTime, Space.World);
            knockbackVelocity = Vector3.Lerp(knockbackVelocity, Vector3.zero, knockbackDecay * Time.fixedDeltaTime);
            return;
        }

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
            float sqrDistance = (target.GetPos() - transform.position).sqrMagnitude;
            float attackRadiusSum = target.GetActionRadius() + this.enemyRadius + this.attackRange;
            // or tries to attack or moves
            if (sqrDistance <= attackRadiusSum * attackRadiusSum)
            {
                // timer only ticks when can attack
                attackTimer -= Time.fixedDeltaTime;
                if (this.attackTimer <= 0.0f)
                {
                    // TODO: attack sound
                    attackTimer = 1.0f;
                    target.DoDamage(this.damage);
                    Vector3 knockbackDir = (transform.position - target.GetPos()).normalized;
                    knockbackVelocity = knockbackDir * knockbackForce;
                }
            }
            else
            {
                // timer resets if cannot attack
                attackTimer = 1.0f;
                Vector3 movement = (target.GetPos() - transform.position).normalized;
                transform.Translate(movement * (speed * Time.fixedDeltaTime), Space.World);
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
