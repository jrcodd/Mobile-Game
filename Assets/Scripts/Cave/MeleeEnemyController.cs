using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyController : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public AttackIndicatorController attackIndicator;
    public Renderer enemyRenderer;
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
    private Material originalMaterial;
    private Material blackMaterial;

    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = moveSpeed;
        attackIndicator.Hide();

        originalMaterial = enemyRenderer.sharedMaterial;
        blackMaterial = new Material(Shader.Find("Standard"));
        blackMaterial.color = Color.black;

        // Set NavMeshAgent settings
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
        agent.stoppingDistance = attackRange * 0.25f;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (agent.isStopped)
        {
            agent.velocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
        switch (currentState)
        {
            case EnemyState.Idle:
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
                break;

            case EnemyState.QuickAttacking:
            case EnemyState.HeavyAttacking:
                PerformAttack();
                break;

            case EnemyState.Recovering:
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
        float windupTime = currentState == EnemyState.QuickAttacking ? quickAttackWindup : heavyAttackWindup;
        float damage = currentState == EnemyState.QuickAttacking ? quickAttackDamage : heavyAttackDamage;

        attackTimer += Time.deltaTime;

        if (isWindingUp)
        {
            float windupProgress = attackTimer / windupTime;
            attackIndicator.SetFillAmount(windupProgress);

            if (attackTimer >= windupTime)
            {
                isWindingUp = false;
                attackTimer = 0f;
                DealDamage(damage);
            }
        }
        else
        {
            float recoveryTime = currentState == EnemyState.QuickAttacking ? quickAttackRecovery : heavyAttackRecovery;
            currentState = EnemyState.Recovering;
            attackIndicator.Hide();
            enemyRenderer.material = blackMaterial;
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
        if (attackIndicator.GetComponent<BoxCollider>() != null)
        {
            BoxCollider collider = attackIndicator.GetComponent<BoxCollider>();
            if (collider.bounds.Contains(player.position))
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
        enemyRenderer.material = originalMaterial;
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