using UnityEngine;
using UnityEngine.AI;

public class RockGolemBoss : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public AttackIndicatorController attackIndicator;
    public Renderer enemyRenderer;
    private Animator animator;

    private Transform player;

    [Header("Enemy Stats")]
    public float moveSpeed = 4.5f;
    public float detectionRange = 40f;
    public float attackRange = 10f;

    [Header("Quick Attack")]
    public float quickAttackDamage = 25f;
    public float quickAttackWindup = 1f;
    public float quickAttackRecovery = 0.5f;

    [Header("Heavy Attack")]
    public float heavyAttackDamage = 125f;
    public float heavyAttackWindup = 4f;
    public float heavyAttackRecovery = 5.5f;


    [Header("Positioning")]
    public float minDistanceBetweenEnemies = 0f;

    private enum EnemyState { Idle, Chasing, QuickAttacking, HeavyAttacking, Recovering }
    private EnemyState currentState = EnemyState.Idle;

    private float attackTimer = 0f;
    private bool isWindingUp = false;


    private Rigidbody rb;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        agent.speed = moveSpeed;
        attackIndicator.Hide();
        animator = GetComponent<Animator>();



        // Set NavMeshAgent settings
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
        agent.stoppingDistance = attackRange * 0.25f;
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").transform != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (agent.isStopped)
        {
            agent.velocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
        switch (currentState)
        {
            case EnemyState.Idle:
                if(animator == null)
                {
                    animator = gameObject.GetComponent<Animator>();
                }
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isAttacking2", false);
                if (distanceToPlayer <= detectionRange)
                {
                    currentState = EnemyState.Chasing;
                }
                break;

            case EnemyState.Chasing:
                animator.SetBool("isWalking", true);
                ChasePlayer();
                if (distanceToPlayer <= attackRange)
                {
                    PrepareAttack();
                }
                break;

            case EnemyState.QuickAttacking:
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking1", true);
                PerformAttack();
                break;
            case EnemyState.HeavyAttacking:
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking2", true);
                PerformAttack();
                break;

            case EnemyState.Recovering:
                animator.SetBool("isAttacking2", false);
                animator.SetBool("isAttacking1", false);
                animator.SetBool("isWalking", false);
                Recover();
                break;
        }
    }

    private void ChasePlayer()
    {
        if (agent.isStopped) agent.isStopped = false;

        Vector3 targetPosition = CalculateTargetPosition();
        agent.SetDestination(targetPosition);
    }

    private Vector3 CalculateTargetPosition()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position - 0.9f * attackRange * directionToPlayer;

        // Avoid other enemiese
        /*
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, minDistanceBetweenEnemies);
        foreach (Collider enemyCollider in nearbyEnemies)
        {
            if (enemyCollider.gameObject != gameObject && enemyCollider.GetComponent<MeleeEnemyController>() != null)
            {
                Vector3 awayFromEnemy = transform.position - enemyCollider.transform.position;
                targetPosition += awayFromEnemy.normalized * (minDistanceBetweenEnemies - awayFromEnemy.magnitude);
            }
        }
        */
        return targetPosition;
    }

    private void PrepareAttack()
    {
        agent.isStopped = true;
        currentState = Random.value > 0.7f ? EnemyState.HeavyAttacking : EnemyState.QuickAttacking;
        attackTimer = 0f;
        isWindingUp = true;
        attackIndicator.Show();

        // Make the enemy look at the player
        transform.LookAt(player.position);
    }

    private void PerformAttack()
    {
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
                attackTimer = 0f;
                isWindingUp = false;

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
        Vector3 enemyToPlayer = player.position - transform.position;
        float distanceToPlayer = enemyToPlayer.magnitude;
        if(attackIndicator.GetComponent<BoxCollider>() != null)
        {
            BoxCollider collider = attackIndicator.GetComponent<BoxCollider>();
            if(collider.bounds.Contains(player.position))
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
