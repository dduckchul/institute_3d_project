using System;
using System.Collections;
using UnityEngine;

// 문 스위치
public class DoorSwitch : MonoBehaviour
{
    // 열릴 문 지정하기
    public GameObject target;
    // 태그 확인하기
    public String tagName = "Player";
    // 열리는 각도 조정
    public float openAngle = 90f;
    // 열리는 시간 설정
    public float openDuration = 1f;

    private bool isOpening;
    private bool isOpen;
    
    // 플레이어 태그를 확인하여 디버그 로그를 출력하는 함수
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagName))
        {
            OpenDoor(target);
        }
    }

    // 문 부드럽게 열리기
    public void OpenDoor(GameObject door)
    {
        if (door == null || isOpening || isOpen)
        {
            return;
        }

        StartCoroutine(OpenDoorSmoothly(door.transform));
    }

    // 문 부드럽게 열리는 코루틴
    private IEnumerator OpenDoorSmoothly(Transform door)
    {
        // 문 열리는것 중복으로 하지 않게끔
        isOpening = true;

        // 로테이션 이동 시작
        Quaternion startRotation = door.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, openAngle, 0f);
        float elapsedTime = 0f;

        // 1초동안 열리는 애니메이션 동작
        while (elapsedTime < openDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / Mathf.Max(openDuration, 0.01f));
            door.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        door.localRotation = endRotation;
        isOpening = false;
        isOpen = true;
    }
}
