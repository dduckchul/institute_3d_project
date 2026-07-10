using System;
using System.Collections;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    public GameObject target;
    public String tagName = "Player";
    public float openAngle = 90f;
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

    public void OpenDoor(GameObject door)
    {
        if (door == null || isOpening || isOpen)
        {
            return;
        }

        StartCoroutine(OpenDoorSmoothly(door.transform));
    }

    private IEnumerator OpenDoorSmoothly(Transform door)
    {
        isOpening = true;

        Quaternion startRotation = door.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, openAngle, 0f);
        float elapsedTime = 0f;

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
