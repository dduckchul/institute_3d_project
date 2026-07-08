using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    
    public Vector3 offset = new Vector3(0f, 5f, -7f);
    
    // void Update()
    // {
    //     transform.position = target.position + offset;
    // }

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
