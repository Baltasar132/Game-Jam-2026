using UnityEngine;

public class BuildingImpl : MonoBehaviour, IHealthBuilding
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private float buildingRadius = 2.0f;
    [SerializeField] public BuildingSize size = new(1);
    [SerializeField] public Vector3 center;
    [SerializeField] public Vector3 placePoint;

    public void FixedUpdate()
    {
        if (this.currentHealth <= 0.0f)
        {
            Destroy(this.transform.parent.gameObject);
            Builds.RemoveBuilding(center, placePoint, size);
        }
        else
        {
            FixedUpdate2();
        }
    }

    public virtual void FixedUpdate2() { }

    public float GetMaxHealth()
    {
        return this.maxHealth;
    }

    public float GetCurrentHealth()
    {
        return this.currentHealth;
    }

    public void DoDamage(float damage)
    {
        this.currentHealth -= damage;
    }

    public float GetActionRadius()
    {
        return this.buildingRadius;
    }

    public Vector3 GetPos()
    {
        return this.GetComponentInParent<Transform>().position;
    }
}