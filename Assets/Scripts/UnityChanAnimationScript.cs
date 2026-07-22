using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnityChanAnimationScript : MonoBehaviour
{
    [SerializeField] private TransformMovement movement;
    [SerializeField] private Animator animator;

    private static readonly int VelocityHash = Animator.StringToHash("Velocity");

    private void Awake()
    {
        if (movement == null)
        {
            movement = GetComponent<TransformMovement>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // The sample character prefab has its Animator disabled for preview use.
        animator.enabled = true;
    }

    private void LateUpdate()
    {
        if (movement == null)
        {
            animator.SetFloat(VelocityHash, 0f);
            return;
        }


        animator.SetFloat(VelocityHash, movement.Velocity.magnitude);
    }

    public void ActiveJump()
    {
        animator.SetTrigger("DoJump");
    }
}
