using UnityEngine;

/// <summary>
/// This script is for the player controller. It handles the player's movement, dodging, and attacking. (not in use bcause I do it server side now)
/// </summary>
/// <author>Jackson Codd</author>
/// <version>1.0 Build 2024.11.24</version>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerControllerNew : MonoBehaviour
{
    [Header("UI")]
    /// <summary>
    /// The joystick that the player will use to move
    /// </summary>
    [SerializeField] private FixedJoystick joystick;

    /// <summary>
    /// The animator object that will animate the player
    /// </summary>
    [SerializeField] private Animator animator;

    [Header("PlayerSettings")]

    /// <summary>
    /// The speed that the player will move
    /// </summary>
    [SerializeField] private float moveSpeed = 5f;

    /// <summary>
    /// The speed that the player will dodge
    /// </summary>
    [SerializeField] private float dodgeSpeed = 20f;

    /// <summary>
    /// The duration of the dodge
    /// </summary>
    [SerializeField] private float dodgeDuration = 0.2f;

    /// <summary>
    /// The cooldown of the dodge
    /// </summary>
    [SerializeField] private float dodgeCooldown = 1f;

    /// <summary>
    /// The rate that the player will hit
    /// </summary>
    [SerializeField] private float hitRate = 1f;

    /// <summary>
    /// The range that the player will hit in
    /// </summary>
    [SerializeField] private float hitRange = 3f;

    [Header("Prefabs")]
    /// <summary>
    /// The slash animation that will play when the player attacks
    /// </summary>
    [SerializeField] private GameObject slashAnimation;

    [Header("Camera")]
    /// <summary>
    /// The main camera
    /// </summary>
    [SerializeField] private Camera mainCamera;

    /// <summary>
    /// The character controller
    /// </summary>
    private CharacterController controller;

    /// <summary>
    /// The move direction of the player
    /// </summary>
    private Vector3 moveDirection;

    /// <summary>
    /// The last move direction of the player
    /// </summary>
    private Vector3 lastMoveDirection;

    /// <summary>
    /// The direction that the player will dodge
    /// </summary>
    private Vector3 dodgeDirection;

    /// <summary>
    /// Whether the player is dodging
    /// </summary>
    private bool isDodging;

    /// <summary>
    /// The time of the last dodge
    /// </summary>
    private float lastDodgeTime;

    /// <summary>
    /// The time of the last hit
    /// </summary>
    private float lastHitTime;

    /// <summary>
    /// The target enemy that the player will attack
    /// </summary>
    private Transform targetEnemy;

    /// <summary>
    /// The damage that the player will deal
    /// </summary>
    private float damage;

    /// <summary>
    /// The timer for the dodge
    /// </summary>
    private float dodgeTimer;

    /// <summary>
    /// When the script is enabled, initialize the components
    /// </summary>
    private void OnEnable()
    {
        InitializeComponents();
    }

    /// <summary>
    /// When the script is enabled, set up the damage and health
    /// </summary>
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

    /// <summary>
    /// When the script is enabled, set up the damage and health
    /// </summary>
    private void SetupDamageAndHealth()
    {
        damage = PlayerCharacter.Singleton.equippedItem.damage == 0 ? 1 : PlayerCharacter.Singleton.equippedItem.damage * (1f + 0.01f * PlayerCharacter.Singleton.currentLevel);
        gameObject.GetComponent<Health>().maxHealth = PlayerCharacter.Singleton.GetHealth();
    }

    /// <summary>
    /// Every frame handle moevement. 
    /// </summary>
    private void Update()
    {
        HandleMovement();
        HandleDodge();
        FindNearestEnemy();
        AttackEnemy();
    }

    /// <summary>
    /// move the player based on joystick input
    /// </summary>
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
    
    /// <summary>
    /// dode action
    /// </summary>
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
    /// <summary>  
    /// Start the dodge player movement
    /// </summary>
    private void StartDodge(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        dodgeDirection = direction;
        isDodging = true;
        dodgeTimer = 0f;
        lastDodgeTime = Time.time;
        animator.SetBool("Dodging", true);
    }
    /// <summary>
    /// End the dodge player movement
    /// </summary>
    private void EndDodge()
    {
        isDodging = false;
        animator.SetBool("Dodging", false);
    }

    /// <summary>
    /// Find the nearest enemy
    /// </summary>
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

    /// <summary>
    /// Attack the enemy
    /// </summary>
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