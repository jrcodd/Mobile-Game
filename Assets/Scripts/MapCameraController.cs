using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MapCameraController : MonoBehaviour
{ 
    public float panSpeed = 20f;
    public float zoomSpeed = 2f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    public float boundaryPadding = 5f;
    public LayerMask tileLayer;
    public float raycastDistance = 100f;
    public static bool isFrozen;

    private Camera cam;
    private Vector2 lastPanPosition;
    private bool isPanning = false;

    private float mapWidth = 150f;
    private float mapHeight = 150f;

    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    private InputAction touchDeltaAction;


    private void Awake()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main camera not found! Please ensure there's a camera tagged as 'MainCamera' in the scene.");
            enabled = false;
            return;
        }

        // Enable enhanced touch support
        EnhancedTouchSupport.Enable();

        // Set up input actions
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found! Please add it to the same GameObject as this script.");
            enabled = false;
            return;
        }

        touchPositionAction = playerInput.actions["TouchPosition"];
        touchPressAction = playerInput.actions["TouchPress"];
        touchDeltaAction = playerInput.actions["TouchDelta"];
    }

    private void OnEnable()
    {
        touchPressAction.performed += OnTouchPressed;
        touchPressAction.canceled +=  OnTouchReleased;
        Touch.onFingerMove += OnFingerMove;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= OnTouchPressed;
        touchPressAction.canceled -= OnTouchReleased;
        Touch.onFingerMove -= OnFingerMove;
    }

    private void Update()
    {
        if (cam == null) return;

        HandleZooming();
    }

    private void OnTouchPressed(InputAction.CallbackContext context)
    {
        if (gameObject.activeSelf)
        {
            if (isFrozen) return;

            if (Touch.activeTouches.Count == 1)
            {
                lastPanPosition = touchPositionAction.ReadValue<Vector2>();
                isPanning = true;
            }
        }
    }

    private void OnFingerMove(Finger finger)
    {
        if (gameObject.activeSelf)
        {
            if (!isFrozen)
            {
                if (Touch.activeTouches.Count == 1)
                {
                    Vector2 delta = touchDeltaAction.ReadValue<Vector2>();
                    Vector3 move = new Vector3(-delta.x, 0, -delta.y) * panSpeed * Time.deltaTime;
                    Vector3 newPosition = transform.localPosition + move;
                    newPosition.x = Mathf.Clamp(newPosition.x, -boundaryPadding, boundaryPadding);
                    newPosition.z = Mathf.Clamp(newPosition.z, -boundaryPadding, boundaryPadding);

                    transform.localPosition = newPosition;
                }
            }
        }
    }

    private void OnTouchReleased(InputAction.CallbackContext context)
    {
        if (gameObject.activeSelf)
        {
            isPanning = false;
            if (touchDeltaAction.ReadValue<Vector2>().magnitude < 0.05f)
            {
                HandleTileSelection(touchPositionAction.ReadValue<Vector2>());
            }
        }
        
    }

    private void HandleZooming()
    {
        if (gameObject.activeSelf)
        {
            if (!isFrozen)
            {
                if (Touch.activeTouches.Count == 2)
                {
                    var touch1 = Touch.activeTouches[0];
                    var touch2 = Touch.activeTouches[1];

                    Vector2 touch1PrevPos = touch1.screenPosition - touch1.delta;
                    Vector2 touch2PrevPos = touch2.screenPosition - touch2.delta;

                    float prevMagnitude = (touch1PrevPos - touch2PrevPos).magnitude;
                    float currentMagnitude = (touch1.screenPosition - touch2.screenPosition).magnitude;

                    float difference = currentMagnitude - prevMagnitude;

                    Zoom(difference * zoomSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void Zoom(float increment)
    {
        if (gameObject.activeSelf)
        {
            if (cam != null && cam.orthographic)
            {
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, minZoom, maxZoom);
            }
            else if (cam != null)
            {
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - increment, minZoom, maxZoom);
            }
        }
    }

    private void HandleTileSelection(Vector2 screenPosition)
    {
        if (gameObject.activeSelf)
        {
            if (cam == null) return;

            Ray ray = cam.ScreenPointToRay(screenPosition);
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red, 1f);

            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, tileLayer))
            {
                Vector3 hitPoint = hit.point;
                int tileX = Mathf.FloorToInt(hitPoint.x / 10);
                int tileZ = Mathf.FloorToInt(hitPoint.z / 10);

                Region hitTile = hit.collider.GetComponent<Region>();
                if (hitTile != null)
                {
                    hitTile.DelayedHighlight(0.1f);
                    isFrozen = true;
                }
                else
                {
                    Debug.LogWarning("Hit object does not have a Tile component.");
                }
            }
            else
            {
                isFrozen = false;
                if (Map.Singleton != null && Map.Singleton.isActiveAndEnabled) Map.Singleton.HideDashedLineAndButton();
                if (Base.Singleton != null && Base.Singleton.isActiveAndEnabled) Base.Singleton.HideButton();
            }
        }
    }
}
