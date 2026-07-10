using UnityEngine;

public class EnterWarning : MonoBehaviour
{
    // 플레이어 태그를 확인하여 디버그 로그를 출력하는 함수
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("위험 구역 진입");
        }
    }
}
