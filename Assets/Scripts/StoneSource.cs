using UnityEngine;

public class StoneSource : MonoBehaviour
{
    public float radius = 1.0f;
    public int quantity = 100;
    public bool alive = true;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (quantity == 0 && alive)
        {
            // TODO: falling animation
            // TODO: update wood source node tree
            Builds.RemoveStone(this.transform.parent.position);
            Destroy(this.transform.parent.gameObject, 0.2f);
            alive = false;
        }
    }
}
