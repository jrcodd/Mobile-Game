using UnityEngine;

/// <summary>
/// This script is for the camera controller. The camera will follow the player and focus on the boss when the player is near the boss.
/// </summary>
/// <author>Jackson Codd</author>
/// <version>1.0 Build 2024.11.24</version>
public class CameraControllerNew : MonoBehaviour
{

    [Header("Target Settings")]
    /// <summary>
    /// The target that the camera will follow
    /// </summary>
    [SerializeField] private Transform target;

    /// <summary>
    /// The offset of the camera
    /// </summary>
    [SerializeField] private float cameraOffset = 2f;

    [Header("Boss Focus Settings")]

    /// <summary>
    /// The radius that the camera will check for the boss
    /// </summary>
    [SerializeField] private float bossCheckRadius = 30f;

    /// <summary>
    /// The speed that the camera will lerp to the boss
    /// </summary>
    [SerializeField] private float bossFocusLerpSpeed = 2f;

    /// <summary>
    /// The offset of the camera when focusing on the boss
    /// </summary>
    [SerializeField] private Vector3 bossFocusOffset = new Vector3(0, 1, -8);

    /// <summary>
    /// The layer that the boss is on
    /// </summary>
    [SerializeField] private LayerMask bossLayer;

    /// <summary>
    /// The current velocity of the camera
    /// </summary>
    private Vector3 currentVelocity;

    /// <summary>
    /// The current boss target
    /// </summary>
    private Transform currentBossTarget;

    /// <summary>
    /// The progress of the lerp to the boss
    /// </summary>
    private float bossLerpProgress = 0f;

    /// <summary>
    /// The starting rotation of the camera
    /// </summary>
    private Quaternion startRotation;

    /// <summary>
    /// move the camera to the player when the script is enabled
    /// </summary>
    private void OnEnable()
    {
        if (target == null)
        {
            FindPlayer();
        }
        startRotation = transform.rotation;
    }
    
    /// <summary>
    ///go to the boss and play an animation then go to the player to show where the boss is
    /// </summary>
    private void Intro()
    {
        if (bossLerpProgress <1)
            {
            if (currentBossTarget == null)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<Health>().isBoss)
                    {
                        currentBossTarget = enemy.transform;
                        Intro();
                    }
                }
            }
            if(currentBossTarget == null)
            {
                return;
            }
            Vector3 desiredPosition = target.position + Vector3.up * cameraOffset;
            Vector3 bossFocusPosition = currentBossTarget.position + cameraOffset * 2 * Vector3.up;
            transform.LookAt(currentBossTarget);
            desiredPosition = Vector3.Lerp(desiredPosition, bossFocusPosition, bossLerpProgress);
            bossLerpProgress+= Time.deltaTime * bossFocusLerpSpeed;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 0.1f);
        }
    }

    /// <summary>
    /// Update the camera position
    /// </summary>
    private void LateUpdate()
    {
        if (target == null)
        {
            FindPlayer();
            return;
        }
        UpdateBossTarget();
        UpdateCameraPosition();
    }

    /// <summary>
    /// initialize the player and target objects
    /// </summary>
    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    /// <summary>
    /// Update the boss target to see if it should focus on the boss
    /// </summary>
    private void UpdateBossTarget()
    {
        if (target != null)
        {
            
            Collider[] bossColliders = Physics.OverlapSphere(target.position, bossCheckRadius, bossLayer);
            if (bossColliders != null && bossColliders.Length > 0)
            { 
                currentBossTarget = bossColliders.Length > 0 ? bossColliders[0].transform : null;

                if (currentBossTarget != null)
                {
                    bossLerpProgress = Mathf.Min(bossLerpProgress + Time.deltaTime * bossFocusLerpSpeed, 1f);
                }
                else
                {
                    bossLerpProgress = Mathf.Max(bossLerpProgress - Time.deltaTime * bossFocusLerpSpeed, 0f);
                }
            }
        }
    }

    /// <summary>
    /// Update the camera position
    /// </summary>
    private void UpdateCameraPosition()
    {
        bossLerpProgress = 1;
        if (bossLerpProgress < 1)
        {
            Intro();
        }
        else
        {
            if (transform.rotation != startRotation)
            {
                transform.rotation = startRotation;
            }

            Vector3 desiredPosition = target.position + Vector3.up * cameraOffset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 0.1f);          
        }
    }
}