using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 5;

    void FixedUpdate()
    {
        Vector2 movement2d = InputSystem.actions["Move"].ReadValue<Vector2>();
        Vector3 movement = new Vector3(movement2d.x, 0, movement2d.y);

        // ResourceManager.WalkingSound(movement.magnitude);

        transform.Translate(movement * speed * Time.fixedDeltaTime, Space.World);
    }
}
