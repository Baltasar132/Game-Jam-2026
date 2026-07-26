using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 5f, rotationSpeed = 10f;
    public Animator animator;

    

    void FixedUpdate()
    {
        Vector2 input = InputSystem.actions["Move"].ReadValue<Vector2>();
        Vector3 movement = new(input.x, 0, input.y);

        if (movement != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed * Time.fixedDeltaTime);

        transform.Translate(movement * speed * Time.fixedDeltaTime, Space.World);
        animator.SetBool("Running", movement != Vector3.zero);
    }

    
}