using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyControllerAnimated : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public AttackIndicatorController attackIndicator;
    public Renderer enemyRenderer;
    public Animator animator;
    private Transform player;


    [Header("Enemy Stats")]
    public float moveSpeed = 3.5f;
    public float detectionRange = 10f;
    public float attackRange = 2f;

    [Header("Quick Attack")]
    public float quickAttackDamage = 10f;
    public float quickAttackWindup = 0.5f;
    public float quickAttackRecovery = 0.5f;

    [Header("Heavy Attack")]
    public float heavyAttackDamage = 25f;
    public float heavyAttackWindup = 1.5f;
    public float heavyAttackRecovery = 1f;


    [Header("Positioning")]
    public float minDistanceBetweenEnemies = 1.5f;

    private enum EnemyState { Idle, Chasing, QuickAttacking, HeavyAttacking, Recovering }
    private EnemyState currentState = EnemyState.Idle;

    private float attackTimer = 0f;
    private bool isWindingUp = false;

    private Collider attackIndicatorCollider;
    private Collider playerCollider;


    private void OnEnable()
    {
        
        animator = animator != null ? animator : GetComponent<Animator>();
        agent.speed = moveSpeed;
        attackIndicator.Hide();


        // Set NavMeshAgent settings
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
        agent.stoppingDistance = attackRange * 0.25f;
        attackIndicatorCollider = attackIndicator.GetComponent<BoxCollider>();
        attackIndicatorCollider.isTrigger = false;

    }

    private void Update()
    {
        if(player == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
            playerCollider = player.GetComponent<BoxCollider>();
        }



        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (agent.isStopped)
        {
            animator.SetBool("isRunning", false);
        }
        switch (currentState)
        {
            case EnemyState.Idle:
                animator.SetBool("isRunning", false);
                if (distanceToPlayer <= detectionRange)
                {
                    currentState = EnemyState.Chasing;
                }
                break;

            case EnemyState.Chasing:
                ChasePlayer();
                if (distanceToPlayer <= attackRange)
                {

                    PrepareAttack();
                }
                else
                {
                    animator.SetBool("isRunning", true);

                }
                break;

            case EnemyState.QuickAttacking:
                animator.SetBool("lightAttack", true);
                PerformAttack();
                break;
            case EnemyState.HeavyAttacking:
                animator.SetBool("heavyAttack", true);
                PerformAttack();
                break;

            case EnemyState.Recovering:
                animator.SetBool("heavyAttack", false);
                animator.SetBool("lightAttack", false);
                animator.SetBool("isRunning", false);
                Recover();
                break;
        }
    }

    private void ChasePlayer()
    {
        if (agent.isStopped) agent.isStopped = false;

        if (Time.frameCount % 60 == 0) agent.destination = (player.position);
    }


    private void PrepareAttack()
    {
        agent.isStopped = true;
        currentState = Random.value > 0.7f ? EnemyState.HeavyAttacking : EnemyState.QuickAttacking;
        attackTimer = 0f;
        isWindingUp = true;
        attackIndicator.Show();

        // Make the enemy look at the player
        Vector3 lookPosition = player.position;
        lookPosition.y = transform.position.y;
        transform.LookAt(lookPosition);
    }

    private void PerformAttack()
    {
        attackIndicatorCollider.isTrigger = true;  

        float windupTime = currentState == EnemyState.QuickAttacking ? quickAttackWindup : heavyAttackWindup;
        float damage = currentState == EnemyState.QuickAttacking ? quickAttackDamage : heavyAttackDamage;

        attackTimer += Time.deltaTime;

        if (isWindingUp)
        {
            float windupProgress = attackTimer / windupTime;
            attackIndicator.SetFillAmount(windupProgress);

            if (attackTimer >= windupTime)
            {
 
                DealDamage(damage);
                isWindingUp = false;
                attackTimer = 0f;
            }
        }
        else
        {
            float recoveryTime = currentState == EnemyState.QuickAttacking ? quickAttackRecovery : heavyAttackRecovery;
            currentState = EnemyState.Recovering;
            attackIndicator.Hide();
        }
    }

    private void Recover()
    {
        float recoveryTime = currentState == EnemyState.QuickAttacking ? quickAttackRecovery : heavyAttackRecovery;
        attackTimer += Time.deltaTime;

        if (attackTimer >= recoveryTime)
        {
            EndAttack();
        }
    }

    private void DealDamage(float damage)
    {
        if (IsPlayerInAttackRange())
        {
            player.GetComponent<Health>()?.TakeDamage(damage);
        }

    }

    private bool IsPlayerInAttackRange()
    {
        attackIndicatorCollider.isTrigger = true;
        if (attackIndicatorCollider.bounds.Intersects(playerCollider.bounds)){
            return true;
        }

        //idk y we cant just do bounds contains bounds but this way works and that way doent
        Collider[] hitColliders = Physics.OverlapBox(
            attackIndicator.transform.position,
            attackIndicator.gameObject.GetComponent<BoxCollider>().size,
            attackIndicator.transform.rotation);
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == player.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    private void EndAttack()
    {
        currentState = EnemyState.Chasing;
        agent.isStopped = false;
        attackIndicator.Hide();
        attackTimer = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}