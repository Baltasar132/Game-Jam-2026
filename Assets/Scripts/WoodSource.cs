using UnityEngine;

public class WoodSource : MonoBehaviour
{
    public float radius = 1.0f;
    public int quantity = 100;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (quantity == 0)
        {
            // TODO: falling animation
            // TODO: update wood source node tree
            Destroy(this.gameObject, 0.2f);
        }
    }
}
