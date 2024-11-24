using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyControllerMultiplayer))]
public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AttackIndicatorController attackIndicator;
    [SerializeField] private EnemyControllerMultiplayer enemyController;
    private Vector3 lastPosition;

    private void OnEnable()
    {
        lastPosition = transform.position;
    }
    private void FixedUpdate()
    {
        AnimateBasedOnSpeed();
        AnimateAttack();
    }
    public void AnimateBasedOnSpeed()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        animator.SetBool("isRunning", distanceMoved > 0.1f);
        lastPosition = transform.position;
    }
    public void AnimateAttack()
    {
        if (attackIndicator.GetFillAmount() > 0 && attackIndicator.GetFillAmount() < 1)
        {
            if (enemyController.GetAttackType() == 0)
            {
                animator.SetBool("lightAttack", true);
            }
            else
            {
                animator.SetBool("heavyAttack", true);
            }
        }

        else
        {
            animator.SetBool("lightAttack", false);
            animator.SetBool("heavyAttack", false);
        }
    }
}
