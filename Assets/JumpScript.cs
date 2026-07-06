using UnityEngine;
using UnityEngine.InputSystem;

// 이 스크립트는 스페이스바를 누르면 캐릭터가 점프하도록 만듭니다.
// 유니티 6에서 Input System 패키지를 사용하는 경우에 맞춘 코드입니다.
public class JumpScript : MonoBehaviour
{
    // 점프 높이를 제어하는 변수입니다.
    // 인스펙터에서 숫자를 조절하면 점프 세기를 변경할 수 있습니다.
    public float jumpForce = 5f;

    // 캐릭터가 땅에 있는지 확인하는 변수입니다.
    private bool isGrounded = true;

    // Rigidbody 컴포넌트를 저장하는 변수입니다.
    private Rigidbody rb;

    void Start()
    {
        // 시작할 때 이 오브젝트의 Rigidbody 컴포넌트를 찾아서 저장합니다.
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody 컴포넌트가 필요합니다. 이 스크립트를 사용하는 오브젝트에 Rigidbody를 추가하세요.");
        }
    }

    void Update()
    {
        // Input System을 사용할 때는 Keyboard.current를 사용합니다.
        // Keyboard.current가 null이 아니고 스페이스키를 눌렀다면 점프합니다.
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded && rb != null)
        {
            Jump();
        }
    }

    private void Jump()
    {
        // 현재 속도를 유지하면서 위 방향으로 점프 속도를 설정합니다.
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);

        // 점프했으므로 땅에 있지 않다고 표시합니다.
        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 Ground 태그를 가지고 있으면 착지로 판단합니다.
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
