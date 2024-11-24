using UnityEngine;

[RequireComponent(typeof(PlayerMovementMultiplayer))]
public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float playerMoveSpeed;
    [SerializeField] private GameObject slashAnimation;
    [SerializeField] private PlayerMovementMultiplayer playerMovementMultiplayer;

    [SerializeField]private float attackRange = 3f;
    private SphereCollider detectionCollider;
    [SerializeField] private float hitRate = 2f;

    private float lastHitTime;
    private Vector3 lastPosition;
    private bool isAttacking;

    public void SetAttacking(bool _isAttacking)
    {
        isAttacking = _isAttacking;
    }

    private void OnEnable()
    {
        lastPosition = transform.position;
        detectionCollider = gameObject.AddComponent<SphereCollider>();
        detectionCollider.radius = attackRange;
        detectionCollider.isTrigger = true;
        playerMovementMultiplayer = GetComponent<PlayerMovementMultiplayer>();

    }
    public void AnimateBasedOnSpeed()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        animator.SetBool("Running", distanceMoved > 0.1f);
        lastPosition = transform.position;
    }
    public void AnimateBasedOnInput()
    {
        animator.SetBool("Running", playerMovementMultiplayer.GetInputGreaterThanZero());
    }
    public void AnimateAttack()
    {
        
        if (isAttacking) 
        {
            animator.SetBool("Attacking", true);
            if (Time.time >= lastHitTime + (1 / hitRate))
            {
                Vector3 direction = transform.forward;
                Quaternion slashRotation = Quaternion.LookRotation(direction);
                // Play slash animation at the player's position
                GameObject slashAnim = Instantiate(slashAnimation, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), slashRotation);
                Destroy(slashAnim, 2.5f);
                lastHitTime = Time.time;
            }
        }
        else
        {
            animator.SetBool("Attacking", false);
        }
    }
    
}
