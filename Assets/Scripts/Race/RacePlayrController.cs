using UnityEngine;

public class RacePlayrController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float dodgeSpeed = 20f;
    [SerializeField] private float dodgeDuration = 0.2f;
    [SerializeField] private int maxJumps = 3;

    private Vector2 touchStart;
    private bool isSwiping;

    private float lastMoveTime;
    private bool isMoving;
    
    private Vector3 moveDirection;
    private float moveTime;
    private Rigidbody rb;

    int jumpCount = 0;
    
    float groundy;

    void Start()
    {
        groundy = transform.position.y; 
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (!isMoving)
        {
            rb.AddForce(new Vector3(0, -0.2f, 0) * dodgeSpeed);

            HandleMovement();
        }
        else
        {
            PerformDodge();
        }
    }

    // Handle player movement
    private void HandleMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {
                touchStart = _touch.position;
                isSwiping = true;
            }
            else if (_touch.phase == TouchPhase.Moved && isSwiping)
            {
                Vector2 _swipeDelta = _touch.position - touchStart;

                if (_swipeDelta.magnitude > 50)
                {
                    Vector2 swipeDirection = _swipeDelta.normalized;
                    Move(swipeDirection);
                    isSwiping = false;
                }
            }
            else if (_touch.phase == TouchPhase.Ended)
            {
                isSwiping = false;
            }
        }
    }

    private void Move(Vector2 _direction)
    {
        float dirx = 0;
        float diry = 0;
            if(_direction.x > 0.5)
        {
            dirx = 1;
            diry = 0;
        }
        
        else if(_direction.x < -0.5)
        {
            dirx = -1;
            diry = 0;
        }
        else
        {
            if(_direction.y > 0)
            {
                if (jumpCount < maxJumps)
                {
                    jumpCount += 1;
                    dirx = 0;
                    diry = 1;
                }
                
            }

        }
        if(transform.position.y <= groundy)
        {
            jumpCount = 0;
        }
        moveDirection = new Vector3(dirx, diry, 0 );
        isMoving = true;
        moveTime = 0;
        lastMoveTime = Time.time;
        //cameraFollow.SetDodge(false);
        
    }

    private void PerformDodge()
    {
        if (moveDirection.y == 0)
        {

            moveTime += Time.deltaTime;
            if (moveTime < dodgeDuration)
            {
                rb.AddForce(moveDirection * dodgeSpeed*2);
            }
            else
            {
                isMoving = false;
                //cameraFollow.SetDodge(false);
            }
        }
        else
        {
            moveTime += Time.deltaTime;
            if (moveTime/2 < dodgeDuration)
            {
                rb.AddForce(moveDirection * dodgeSpeed);
            }
            else
            {
                isMoving = false;
                //cameraFollow.SetDodge(false);
            }
        }
    }
}
