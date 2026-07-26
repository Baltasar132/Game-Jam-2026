using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;
    public Animator animator;

    void FixedUpdate()
    {
        Vector2 input = InputSystem.actions["Move"].ReadValue<Vector2>();
        Vector3 movement = new(input.x, 0, input.y);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed * Time.fixedDeltaTime);
        }
        animator.SetBool("Running", movement != Vector3.zero);

        Vector3 translation = movement * speed * Time.fixedDeltaTime;
        Vector3 targetPos = transform.position + translation;

        float limitX = (Builds.Width * Builds.CellWidth / 2f) + (2f * Builds.CellWidth);
        float limitZ = (Builds.Height * Builds.CellWidth / 2f) + (2f * Builds.CellWidth);

        targetPos.x = Mathf.Clamp(targetPos.x, -limitX, limitX);
        targetPos.z = Mathf.Clamp(targetPos.z, -limitZ, limitZ);

        transform.position = targetPos;
    }


}