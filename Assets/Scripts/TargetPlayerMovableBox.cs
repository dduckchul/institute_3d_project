using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TargetPlayerMovableBox : MonoBehaviour
{
    public GameObject canMovablePlayer;
    private Rigidbody rb;

    private const RigidbodyConstraints FreezePositionConstraints =
        RigidbodyConstraints.FreezePositionX |
        RigidbodyConstraints.FreezePositionY |
        RigidbodyConstraints.FreezePositionZ;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsMovablePlayer(collision.transform))
        {
            return;
        }

        // 위치 고정만 해제하고 Freeze Rotation 같은 다른 제약은 유지한다.
        rb.constraints &= ~FreezePositionConstraints;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!IsMovablePlayer(collision.transform))
        {
            return;
        }

        // 플레이어가 떨어지면 위치를 다시 고정한다.
        rb.constraints |= FreezePositionConstraints;
    }

    private bool IsMovablePlayer(Transform collidedTransform)
    {
        if (canMovablePlayer == null)
        {
            return false;
        }

        Transform playerTransform = canMovablePlayer.transform;

        return collidedTransform == playerTransform;
    }
}
