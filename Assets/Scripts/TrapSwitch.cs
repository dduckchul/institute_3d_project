using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject switchTarget;

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
