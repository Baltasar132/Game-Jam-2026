using System.Collections.Generic;
using UnityEngine;

public class WoodSource : MonoBehaviour
{
    [SerializeField] public BuildingSize size;
    public int quantity = 100;
    public bool alive = true;
    public List<Vector3> occupying;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (quantity == 0 && alive)
        {
            // TODO: falling animation
            // TODO: update wood source node tree
            Builds.RemoveTree(occupying, size);
            Destroy(this.transform.parent.gameObject, 0.2f);
            alive = false;
        }
    }
}
