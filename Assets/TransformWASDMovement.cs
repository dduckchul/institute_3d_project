using UnityEngine;
using UnityEngine.InputSystem;

public class TransformWASDMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool moveRelativeToCharacter = true;

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

        Space moveSpace = moveRelativeToCharacter ? Space.Self : Space.World;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, moveSpace);
    }
}
