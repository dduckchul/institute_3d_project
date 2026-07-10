using UnityEngine;
using UnityEngine.InputSystem;

// 대상을 클릭하여 이동시키는 컴포넌트
public class ClickToTransformMovement : MonoBehaviour
{
    // 플레이어 1의 Transform 정보
    public Transform player1;
    // 플레이어2의 트랜스폼 정보
    public Transform player2;
    // 메인카메라는 Player1의 카메라
    public Camera mainCamera;
    // 플레이어2의 카메라
    public Camera player2Cam;
    // Raycast 판정할 바닥 레이어
    public LayerMask groundLayer = 1 << 3;
    // 이동속도
    public float moveSpeed = 5f;
    // 회전속도
    public float rotationSpeed = 720f;
    // 근처 거리로 이동하면 멈출 범위
    public float stoppingDistance = 0.05f;

    // 내부적으로 가질 Player1의 대상 포지션
    private Vector3 player1TargetPosition;
    // 내부적으로 가질 Player2의 대상 포지션
    private Vector3 player2TargetPosition;
    // 1Player가 타겟이 있는지 확인
    private bool player1HasTarget;
    // 2Player가 타겟이 있는지 확인
    private bool player2HasTarget;

    void Update()
    {
        // 현재 마우스 입력이 없다면 리턴
        if (Mouse.current == null)
        {
            return;
        }

        // 클릭을 통해 대상 타겟 변경
        UpdateTargetFromClick();
        // 1플레이어 혹은 2플레이어, 타겟을 이동시킨다
        player1HasTarget = MoveToTarget(player1, player1TargetPosition, player1HasTarget);
        player2HasTarget = MoveToTarget(player2, player2TargetPosition, player2HasTarget);
    }

    // 마우스 클릭 이벤트 감지시 타겟을 업데이트한다.
    private void UpdateTargetFromClick()
    {
        // 왼쪽버튼 클릭 감지 없을시 리턴
        if (!Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        // 마우스 위치를 통해  Camera1과 Camera2를 감지
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        // 위치를 가지고 카메라1인지 카메라2인지 변수에 저장하기
        Camera clickCamera = GetClickCamera(mousePosition);

        if (clickCamera == null)
        {
            return;
        }

        // 클릭한 카메라의 Ray 발사하기
        Ray ray = clickCamera.ScreenPointToRay(mousePosition);

        // 대상 카메라와 플레이어 판별하여 이동할 타겟 설정하기
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            SetTarget(clickCamera, hit.point);
        }
    }

    // 클릭한 카메라 가져오기
    private Camera GetClickCamera(Vector2 mousePosition)
    {
        // 카메라 가져올때 방어코드 설정
        if (player2Cam != null && player2Cam.pixelRect.Contains(mousePosition))
        {
            return player2Cam;
        }

        if (mainCamera != null && mainCamera.pixelRect.Contains(mousePosition))
        {
            return mainCamera;
        }

        return null;
    }

    // 카메라와 바닥 맞은 곳 확인하여 타겟 설정.
    private void SetTarget(Camera clickCamera, Vector3 hitPosition)
    {
        if (clickCamera == player2Cam && player2 != null)
        {
            player2TargetPosition = hitPosition;
            player2TargetPosition.y = player2.position.y;
            player2HasTarget = true;
            return;
        }

        if (player1 == null)
        {
            return;
        }

        player1TargetPosition = hitPosition;
        player1TargetPosition.y = player1.position.y;
        player1HasTarget = true;
    }

    // 해당 대상을 이동시키는 메서드
    private bool MoveToTarget(Transform playerTransform, Vector3 targetPosition, bool hasTarget)
    {
        // 플레이어 대상이 아니라면 리턴
        if (playerTransform == null || !hasTarget)
        {
            return false;
        }

        // 타겟 포지션으로 이동
        Vector3 toTarget = targetPosition - playerTransform.position;

        // 이동한 곳이 설정값 이하라면 멈추기.
        if (toTarget.magnitude <= stoppingDistance)
        {
            return false;
        }

        // 이동 방향 구하기
        Vector3 moveDirection = toTarget.normalized;


        // 회전
        Rotate(playerTransform, moveDirection);
        
        // 플레이어 이동
        playerTransform.position += moveDirection * moveSpeed * Time.deltaTime;

        //대상이 남아있다면 true 리턴
        return true;
    }

    // 클릭 위치 방향으로 로테이션
    private void Rotate(Transform playerTransform, Vector3 moveDirection)
    {
        // 이동 방향이 zero면 회전 하지 않음
        if (moveDirection == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        playerTransform.rotation = Quaternion.RotateTowards(
            playerTransform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
