using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    public Transform cameraTransform;

    private CharacterController controller;

    public float gravity = -9.81f;
    float velocityY;
    public float jumpForce = 5f;

    public Animator animator;
    public Transform model;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            input.y += 1;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1;
        if (Keyboard.current.spaceKey.wasPressedThisFrame && controller.isGrounded)
        {
            velocityY = jumpForce;
            animator.SetTrigger("Jump");
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = right * input.x + forward * input.y;

        if (controller.isGrounded && velocityY < 0)
        {
            velocityY = -2f;
        }
        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = Vector3.up * velocityY;

        controller.Move((move.normalized * speed + velocity) * Time.deltaTime);

        Vector3 moveDir = move.normalized;

        if (moveDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);

            model.rotation = Quaternion.Slerp(
                model.rotation,
                targetRotation,
                12f * Time.deltaTime
            );
        }

        float speedValue = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
        animator.SetFloat("Blend", speedValue);
    }
}