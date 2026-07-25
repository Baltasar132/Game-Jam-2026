using UnityEngine;

public interface IHealth
{
    float GetMaxHealth();

    float GetCurrentHealth();

    void DoDamage(float damage);

    float GetActionRadius();

    Vector3 GetPos();
}

public interface IHealthBuilding : IHealth { }
public interface IHealthEnemy : IHealth { }