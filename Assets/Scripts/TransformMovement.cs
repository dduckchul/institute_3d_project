using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어1, 플레이어2 에 따라서 조작하는 키를 다르게 구분하기 위함
public enum PlayerType
{
    Player1, Player2
}

// Transform 으로 이동하게 만드는 스크립트

public class TransformMovement : MonoBehaviour
{
    // Enum으로 플레이어를 지정할수 있도록 하는 변수, 유니티 에디터에서 설정할수 있도록 Public으로 선언
    public PlayerType player;
    // 플레이어의 이동속도
    public float moveSpeed = 5f;
    // 플레이어의 회전 속도
    public float rotationSpeed = 720f;
    // 플레이어가 이동키를 떼었을때, 바로 멈추지 않고, 부드럽게 감속되도록 하는 변수
    public float deceleration = 5f;
    // 플레이어가 대각선으로 이동하는중 키를 떼었을때, 튕겨나가는것처럼 한쪽 방향으로 밀리는 현상을 방지하기 위해, 
    // 이동키를 떼었을때 잠시동안 이동키 입력을 무시하도록 하는 변수
    public float releaseGraceTime = 0.08f;
    
    // 이동 키의 버퍼를 두는 타이머 변수
    private float releaseGraceTimer;
    // 현재 이동 벡터
    private Vector3 currentMoveVector;

    // Animator 같은 다른 컴포넌트에서 현재 이동 속도를 읽을 때 사용한다.
    public Vector3 Velocity => currentMoveVector;

    // MapManager에 경계를 확인하는 함수를 불러오기 위한 변수.
    public MapManager mapManager;

    // 업데이트 함수
    void Update()
    {
        // 현재 입력이 아무것도 없으면 리턴한다.
        if (Keyboard.current == null)
        {
            return;
        }

        // 키의 입력에 따라 노말라이즈 된 이동방향을 가져온다
        Vector3 moveDirection = GetMoveDirectionWithReleaseGrace();
        
        // 이동방향에 따라 현재 이동벡터를 업데이트한다 
        currentMoveVector = UpdateMoveVector(moveDirection);

        // 현재 이동벡터에 따라 플레이어를 이동시킨다
        Move(currentMoveVector);

        // 이동방향에 따라 플레이어를 회전시킨다
        Rotate(currentMoveVector);

        // 클램프 임시 주석처리
        // mapManager.ClampToMapIfOutOfMap(transform);
    }

    // 키의 입력에 따라 노말라이즈 된 이동방향을 가져오는 함수
    // 대각선 이동시, 플레이어가 두개의 키를 거의 동시에 뗐는데
    // 한쪽 방향으로 튕겨나가는 것처럼 밀리는 현상을 방지한다    
    private Vector3 GetMoveDirectionWithReleaseGrace()
    {
        // 현재 키 입력에 따른 방향벡터를 구하는 함수
        Vector3 moveDirection = GetMoveDirection(player);

        // 키 릴리즈 시 타이머를 업데이트하는 함수
        UpdateReleaseGraceTimer();

        // 타이머가 0보다 크면, 이동키를 떼었을때 잠시동안 이동키 입력을 무시하도록 한다
        if (releaseGraceTimer > 0f)
        {
            releaseGraceTimer -= Time.deltaTime;
            return Vector3.zero;
        }

        return moveDirection;
    }

    // 이동키를 떼었을때 타이머를 동작시키게 한다.
    private void UpdateReleaseGraceTimer()
    {
        // 이번 프레임에 이동 키가 떼어졌다면, 타이머를 초기화한다.
        if (WasMoveKeyReleasedThisFrame())
        {
            releaseGraceTimer = releaseGraceTime;
        }

        // 이번 프레임에 이동 키가 눌렸다면, 타이머를 0으로 만든다.
        if (WasMoveKeyPressedThisFrame())
        {
            releaseGraceTimer = 0f;
        }
    }

    // 현재 이동 벡터를 업데이트 한다.
    private Vector3 UpdateMoveVector(Vector3 moveDirection)
    {
        // 이동 키 입력이 없다면, 현재 이동 벡터를 감속시킨다.
        if (moveDirection == Vector3.zero)
        {
            // 감속하는 변수에 따라 이동 속도를 점차 0으로 만든다.
            currentMoveVector = Vector3.Lerp(currentMoveVector, Vector3.zero, Time.deltaTime * deceleration);

            // 0에 가까워지면, 현재 이동 벡터를 0으로 만든다.
            if (currentMoveVector.sqrMagnitude < 0.001f)
            {
                currentMoveVector = Vector3.zero;
            }
        }
        // 이동 키 입력이 있다면, 현재 이동 벡터를 이동 방향에 따라 업데이트 한다.
        else
        {
            currentMoveVector = moveDirection * moveSpeed;
        }

        return currentMoveVector;
    }

    // 현재 이동 벡터에 따라 플레이어를 이동시킨다
    private void Move(Vector3 currentMoveVector)
    {
        transform.position += currentMoveVector * Time.deltaTime;
    }

    // 이동 키 입력에 따라 방향을 전환시킨다.
    private void Rotate(Vector3 moveDirection)
    {
        // 이동 방향이 0이 아니라면, 이동 방향으로 회전시킨다.
        if (moveDirection != Vector3.zero)
        {
            RotateToMoveDirection(moveDirection);
        }
    }

    // Quaternion.LookRotation() 함수를 사용하여 이동 방향으로 회전시킨다.
    private void RotateToMoveDirection(Vector3 moveDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    // 플레이어 타입에 따라 이동 방향을 반환하는 함수
    private Vector3 GetMoveDirection(PlayerType player)
    {
        Vector3 moveDirection = Vector3.zero;

        // 플레이어가 Player1이면 WASD 키를 사용하여 이동 방향을 구한다.
        if(player == PlayerType.Player1)
        {
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
        }
        // 플레이어가 Player2이면 화살표 키를 사용하여 이동 방향을 구한다.
        else if(player == PlayerType.Player2)
        {
            if (Keyboard.current.upArrowKey.isPressed)
            {
                moveDirection += Vector3.forward;
            }

            if (Keyboard.current.downArrowKey.isPressed)
            {
                moveDirection += Vector3.back;
            }

            if (Keyboard.current.leftArrowKey.isPressed)
            {
                moveDirection += Vector3.left;
            }

            if (Keyboard.current.rightArrowKey.isPressed)
            {
                moveDirection += Vector3.right;
            }
        }

        moveDirection.Normalize();

        return moveDirection;
    }

    // 이동키가 이번 프레임에 눌렸는지 확인하는 함수
    private bool WasMoveKeyPressedThisFrame()
    {
        if(player == PlayerType.Player1)
        {
            return Keyboard.current.wKey.wasPressedThisFrame
                || Keyboard.current.sKey.wasPressedThisFrame
                || Keyboard.current.aKey.wasPressedThisFrame
                || Keyboard.current.dKey.wasPressedThisFrame;
        }
        else
        {
            return Keyboard.current.upArrowKey.wasPressedThisFrame
                || Keyboard.current.downArrowKey.wasPressedThisFrame
                || Keyboard.current.leftArrowKey.wasPressedThisFrame
                || Keyboard.current.rightArrowKey.wasPressedThisFrame;            
        }
    }

    // 이동키가 이번 프레임에 떼어졌는지 확인하는 함수
    private bool WasMoveKeyReleasedThisFrame()
    {
        if(player == PlayerType.Player1)
        {
            return Keyboard.current.wKey.wasReleasedThisFrame
                || Keyboard.current.sKey.wasReleasedThisFrame
                || Keyboard.current.aKey.wasReleasedThisFrame
                || Keyboard.current.dKey.wasReleasedThisFrame;
        }
        else
        {
            return Keyboard.current.upArrowKey.wasReleasedThisFrame
                || Keyboard.current.downArrowKey.wasReleasedThisFrame
                || Keyboard.current.leftArrowKey.wasReleasedThisFrame
                || Keyboard.current.rightArrowKey.wasReleasedThisFrame;
        }
    }
}
