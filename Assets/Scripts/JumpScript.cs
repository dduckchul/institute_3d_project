using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어가 점프를 할 수 있도록 만드는 스크립트, MonoBehaviour를 상속받아 컴포넌트로 사용할 수 있다.
// RequireComponent 어트리뷰트를 사용하여 Rigidbody 컴포넌트가 반드시 필요하도록 설정한다.
[RequireComponent(typeof(Rigidbody))] 
public class JumpScript : MonoBehaviour
{
    // 점프 할 때 적용되는 힘의 크기, Unity 에디터에서 편질 할 수 있도록 Public으로 선언 한다.
    public float jumpForce = 5f;

    // 땅에 닿았는지 판정하는 변수, 초기값은 True로 설정하여 시작시에는 땅에 닿아있다고 가정한다.
    // 런타임에서 변경되는 함수임으로 private로 선언하여 외부에서 접근하지 못하도록 선언한다.
    private bool isGrounded = true;

    // Rigidbody 컴포넌트를 저장할 변수, 점프 시에 Rigidbody에 AddForce로 위 방향으로 힘을 주기위해 사용한다.
    // 해당 컴포넌트 하위에 달려있는 Rigidbody를 쓸것이기 때문에, private로 선언한다.
    private Rigidbody rb;

    private UnityChanAnimationScript unityChanAnimationScript;

    
    // 게임이 처음 실행될때 호출되는 함수, Rigidbody 컴포넌트를 가져와 rb 변수에 저장한다. 
    void Awake()
    {
        unityChanAnimationScript = GetComponent<UnityChanAnimationScript>();

        // Rigidbody 컴포넌트를 가져와 rb 변수에 저장한다.
        rb = GetComponent<Rigidbody>();

        // rb가 null이면 Rigidbody 컴포넌트가 없다는 의미이므로, 에러 메시지를 출력한다.
        if (rb == null)
        {
            Debug.LogError("Rigidbody 컴포넌트가 필요합니다. 이 스크립트를 사용하는 오브젝트에 Rigidbody를 추가하세요.");
        }        
    }
    
    void Start()
    {

    }

    // 1프레임마다 현재 키보드가 눌렸는지 확인하고, 스페이스가 눌렸고, 땅에 닿아있다면 Jump() 함수를 호출하여 점프를 수행한다.

    void Update()
    {
        // Keyboard.current가 null이면 키보드 입력을 받을 수 없으므로, 함수를 종료한다.
        if (Keyboard.current == null) return;

        // 스페이스 키가 눌리지 않았다면 함수를 종료한다.
        if (Keyboard.current.spaceKey.wasPressedThisFrame == false) return;

        // 땅에 닿아있고, Rigidbody 컴포넌트가 존재한다면 Jump() 함수를 호출하여 점프를 수행한다.
        if (isGrounded && rb != null)
        {
            // Jump() 함수를 호출하여 점프를 수행한다.
            Jump();
        }       
    }

    // 실제 점프를 수행하는 함수
    private void Jump()
    {
        // Rigidbody의 linearVelocity를 이용하여 윗쪽 방향으로 힘을 준다.
        // 현재 x와 z축의 속도를 유지한다.
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);

    
        if(unityChanAnimationScript != null)
        {
            unityChanAnimationScript.ActiveJump();            
        }

        // 점프했으므로 땅에 있지 않다고 표시합니다.
        isGrounded = false;
    }

    // 충돌이 발생했을 때 홀출되는 함수 
    void OnCollisionEnter(Collision collision)
    {
        // 닿은 객체의 태그가 Ground라면, isGrounded를 true로 설정하여 땅에 닿았다고 표시한다.
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
