using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 20, -10);

    void Update()
    {
        transform.position = target.position + offset;
    }
}
