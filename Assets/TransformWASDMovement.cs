using UnityEngine;
using UnityEngine.InputSystem;

public class TransformWASDMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        Vector3 moveDirection = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            moveDirection += Vector3.forward;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            moveDirection += Vector3.back;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            moveDirection += Vector3.left;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            moveDirection += Vector3.right;
        }

        if (moveDirection == Vector3.zero)
        {
            return;
        }

        moveDirection.Normalize();

        RotateToMoveDirection(moveDirection);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void RotateToMoveDirection(Vector3 moveDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    // lerp, clamp, Camera offset 변경
}
