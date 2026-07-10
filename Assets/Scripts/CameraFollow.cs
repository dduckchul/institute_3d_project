using UnityEngine;
using UnityEngine.InputSystem;

// 카메라 이동 스크립트, 카메라가 부드럽게 이동하도록 Lerp 함수를 사용하여 카메라를 이동시킨다.
public class CameraFollow : MonoBehaviour
{
    // 카메라가 따라갈 타겟, 플레이어를 지정한다.
    public Transform target;
    
    // 카메라가 타겟과 떨어져 있는 거리, 카메라의 위치를 타겟의 위치에 offset 만큼 떨어진 위치로 설정한다.
    public Vector3 offset = new Vector3(0f, 5f, -7f);

    // 카메라가 타겟을 따라가는 속도, Lerp 함수를 사용하여 카메라를 이동시킬때, 이 속도를 사용하여 카메라가 부드럽게 이동하도록 한다.
    public float followSpeed = 2f;
    
    // LateUpdate를 사용하여 카메라가 버벅이지 않고 부드럽게 따라가도록 한다.
    void LateUpdate()
    {
        SmoothFollow();
    }

    // Lerp 함수를 사용하여 카메라를 부드럽게 따라가도록 하는 함수
    private void SmoothFollow()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }

}
