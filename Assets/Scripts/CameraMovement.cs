using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [Range(0, 10)] public float speed = 5;

    public static CameraMovement INSTANCE;

    void Awake()
    {
        INSTANCE = this;
    }

    void Update()
    {
        Vector2 mov2d = InputSystem.actions["Move"].ReadValue<Vector2>();
        Vector3 mov3d = new(mov2d.x, 0, mov2d.y);

        this.transform.Translate(mov3d * speed * Time.deltaTime, Space.World);
    }
}
