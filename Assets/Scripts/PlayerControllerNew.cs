using UnityEngine;

public class PlayerControllerNew : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Animator animator;

    [Header("PlayerSettings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dodgeSpeed = 20f;
    [SerializeField] private float dodgeDuration = 0.2f;
    [SerializeField] private float dodgeCooldown = 1f;
    [SerializeField] private float hitRate = 1f;
    [SerializeField] private float hitRange = 3f;

    [Header("Prefabs")]
    [SerializeField] private GameObject slashAnimation;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    private CharacterController controller;
    private Vector3 moveDirection;
    private Vector3 lastMoveDirection;
    private Vector3 dodgeDirection;
    private bool isDodging;
    private float lastDodgeTime;
    private float lastHitTime;
    private Transform targetEnemy;
    private float damage;
    private float dodgeTimer;

    private void OnEnable()
    {
        InitializeComponents();
        //SetupDamageAndHealth();
    }

    private void InitializeComponents()
    {
        controller = GetComponent<CharacterController>();
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FixedJoystick>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void SetupDamageAndHealth()
    {
        damage = PlayerCharacter.Singleton.equippedItem.damage == 0 ? 1 : PlayerCharacter.Singleton.equippedItem.damage * (1f + 0.01f * PlayerCharacter.Singleton.currentLevel);
        gameObject.GetComponent<Health>().maxHealth = PlayerCharacter.Singleton.GetHealth();
    }

    private void Update()
    {
        HandleMovement();
        HandleDodge();
        FindNearestEnemy();
        AttackEnemy();
    }

    private void HandleMovement()
    {
        if (isDodging) return;

        // Get raw input from joystick
        Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);

        // Only proceed if there's input
        if (input.magnitude >= 0.1f)
        {
            // Get the forward and right vectors of the camera
            Vector3 forward = mainCamera.transform.forward;
            Vector3 right = mainCamera.transform.right;

            // Project forward and right vectors on the horizontal plane (y = 0)
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            // Calculate the move direction in world space
            moveDirection = (forward * input.y + right * input.x).normalized;

            // Move the player
            controller.Move(moveSpeed * Time.deltaTime * moveDirection);

            // Rotate the player to face the move direction
            if (moveDirection != Vector3.zero)
            {  
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }

            lastMoveDirection = moveDirection;
            animator.SetBool("Running", true);
        }
        else
        {
            moveDirection = Vector3.zero;
            animator.SetBool("Running", false);
        }
    }

    private void HandleDodge()
    {
        if (isDodging)
        {
            dodgeTimer += Time.deltaTime;
            controller.Move(dodgeDirection * dodgeSpeed * Time.deltaTime);

            if (dodgeTimer >= dodgeDuration)
            {
                EndDodge();
            }
            return;
        }

        // Check for tap (quick touch and release)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended && touch.deltaTime < 0.2f && Time.time - lastDodgeTime >= dodgeCooldown)
            {
                StartDodge(lastMoveDirection);
            }
        }
    }

    private void StartDodge(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        dodgeDirection = direction;
        isDodging = true;
        dodgeTimer = 0f;
        lastDodgeTime = Time.time;
        animator.SetBool("Dodging", true);
    }

    private void EndDodge()
    {
        isDodging = false;
        animator.SetBool("Dodging", false);
    }

    private void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float nearestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < nearestDistance)
            {
                nearestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        targetEnemy = nearestEnemy;

        if (enemies.Length == 0)
        {
            GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>().enableDeathUI();
        }
    }

    private void AttackEnemy()
    {
        if (targetEnemy == null) return;

        float distanceToEnemy = Vector3.Distance(transform.position, targetEnemy.position);

        if (distanceToEnemy <= hitRange)
        {
            animator.SetBool("Attacking", true);
            if (Time.time >= lastHitTime + (1f / hitRate))
            {
                Vector3 direction = (targetEnemy.position - transform.position).normalized;
                Quaternion slashRotation = Quaternion.LookRotation(direction);

                // Play slash animation at the player's position
                Instantiate(slashAnimation, transform.position, slashRotation);

                targetEnemy.GetComponent<Health>().TakeDamage(damage * PlayerCharacter.Singleton.GetDamageMultiplier(), true);
                lastHitTime = Time.time;

                // Face the enemy when attacking
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        else
        {
            animator.SetBool("Attacking", false);
        }
    }
}