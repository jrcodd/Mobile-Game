using UnityEngine;

public class CameraControllerNew : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float cameraOffset = 2f;

    [Header("Boss Focus Settings")]
    [SerializeField] private float bossCheckRadius = 30f;
    [SerializeField] private float bossFocusLerpSpeed = 2f;
    [SerializeField] private Vector3 bossFocusOffset = new Vector3(0, 1, -8);
    [SerializeField] private LayerMask bossLayer;


    private Vector3 currentVelocity;
    private Transform currentBossTarget;
    private float bossLerpProgress = 0f;
    Quaternion startRotation;

    private void OnEnable()
    {
        if (target == null)
        {
            FindPlayer();
        }
        startRotation = transform.rotation;
    }
    //go to the boss then go to the player to show where the boss is
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

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

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