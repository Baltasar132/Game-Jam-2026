using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaderaGenerador : MonoBehaviour, IInteractable
{
    public GameObject menu;
    [SerializeField] private float gatherCooldown = 5.0F;
    [HideInInspector] public float cooldown = 0.0F;

    void Start()
    {
        menu.SetActive(false);
        cooldown = this.gatherCooldown;
    }

    void FixedUpdate()
    {
        this.cooldown = MathF.Max(0.0F, this.cooldown - Time.fixedDeltaTime);
    }

    void IInteractable.ShowUI()
    {
        menu.SetActive(true);
    }

    void IInteractable.HideUI()
    {
        menu.SetActive(false);
    }

    void IInteractable.Interact()
    {
        Gather();
    }

    public void Gather()
    {
        if (this.cooldown <= 0.0F)
        {
            ResourceManager.AddWood(ResourceManager.GetLevel(ResType.Wood));
            this.cooldown = this.gatherCooldown;
        }
    }
}
