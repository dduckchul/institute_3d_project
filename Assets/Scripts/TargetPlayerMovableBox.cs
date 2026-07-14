using UnityEngine;

// 대상 플레이어만 움직일수 있는 박스 구현
[RequireComponent(typeof(Rigidbody))]
public class TargetPlayerMovableBox : MonoBehaviour
{
    // 대상 플레이어 설정
    public GameObject canMovablePlayer;
    private Rigidbody rb;

    // Rigidbody 박스 얼리는 조건
    private const RigidbodyConstraints FreezePositionConstraints =
        RigidbodyConstraints.FreezePositionX |
        RigidbodyConstraints.FreezePositionY |
        RigidbodyConstraints.FreezePositionZ;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // 대상 플레이어가 콜리전 엔터시 Freeze 포지션 해제
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsMovablePlayer(collision.transform))
        {
            return;
        }

        // 위치 고정만 해제하고 Freeze Rotation 같은 다른 제약은 유지한다.
        rb.constraints &= ~FreezePositionConstraints;
    }


    // 대상 플레이어가 콜리전에서 빠져나갔을때 대상 Freeze 추가
    private void OnCollisionExit(Collision collision)
    {
        if (!IsMovablePlayer(collision.transform))
        {
            return;
        }

        // 플레이어가 떨어지면 위치를 다시 고정한다.
        rb.constraints |= FreezePositionConstraints;
    }

    // 박스를 움직일수 있는 플레이어인지 체크
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
