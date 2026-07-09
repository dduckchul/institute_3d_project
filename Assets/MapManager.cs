using UnityEngine;

// 맵을 관리하는 클래스, 맵의 경계, 맵에서 Clamp를 활용하여 나가지 못하도록 붙잡아 두는 함수 활용
public class MapManager : MonoBehaviour
{
    // 맵의 경계 지정, 현재 Plane으로 설정되어 있으므로 Y값은 입력하지 않는다, 
    // 직사각형모양의 맵을 Math.abs로 활용할 예정이므로 0,0기준의 절대값만 입력한다
    public Vector3 mapBoundary = new Vector3(10,0,10);

    // 특정 객체가 맵 밖에 있는지를 판별하는 함수 
    public bool IsOutOfMap(Transform playerTransform)
    {
        // 지정한 지도의 X값, Z값 경계를 가져온다.
        float boundaryX = mapBoundary.x;
        float boundaryZ = mapBoundary.z;

        // 플레이어의 X좌표, Z좌표를 가져온다.
        float playerX = Mathf.Abs(playerTransform.position.x);
        float playerZ = Mathf.Abs(playerTransform.position.z);

        // X혹은 Z값이 경계선 밖이라면 True를 반환한다.
        if(playerX > boundaryX || playerZ > boundaryZ)
        {
            return true;            
        }

        return false;
    }

    // 맵 밖이라면 Clamp를 적용하는 함수
    public void ClampToMapIfOutOfMap(Transform playerTransform)
    {
        // 맵 안에 있을경우 예외처리
        if (!IsOutOfMap(playerTransform))
        {
            return;
        }

        // Clamped Position은 -x ~ +x, -z ~ +z 값으로 정한다. y값은 현재 플레이어의 값으로 정한다.
        Vector3 clampedPosition = playerTransform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -mapBoundary.x, mapBoundary.x);
        clampedPosition.y = playerTransform.position.y;
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, -mapBoundary.z, mapBoundary.z);

        playerTransform.position = clampedPosition;
    }
}
