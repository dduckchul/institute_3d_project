using UnityEngine;

// 함정 스위치 스크립트
public class TrapSwitch : MonoBehaviour
{
    // 스위치 눌러서 함정 작동시킬 오브젝트
    public GameObject switchTarget;

    // 스위치 밟았을 때
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Rigidbody rb = switchTarget.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.AddForce(new Vector3 (0,-1000,0), ForceMode.Acceleration);
        }
    }

}
