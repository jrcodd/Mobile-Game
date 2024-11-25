using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

/// <summary>
/// This script is for the camera controller on the map. The camera will pan and zoom in and out when the player uses touch input.
/// </summary>
/// <author>Jackson Codd</author>
/// <version>1.0 Build 2024.11.24</version>
public class MapCameraController : MonoBehaviour
{ 
    [Header("Camera Settings")]
    /// <summary>
    /// The speed that the camera will pan
    /// </summary>
    [SerializeField] private float panSpeed = 20f;

    /// <summary>
    /// The speed that the camera will zoom
    /// </summary>
    [SerializeField] private float zoomSpeed = 2f;

    /// <summary>
    /// The minimum zoom of the camera
    /// </summary>
    [SerializeField] private float minZoom = 5f;

    /// <summary>
    /// The maximum zoom of the camera
    /// </summary>
    [SerializeField] private float maxZoom = 15f;

    /// <summary>
    /// The padding of the boundary
    /// </summary>
    [SerializeField] private float boundaryPadding = 5f;

    /// <summary>
    /// The layer that the tiles are on
    /// </summary>
    [SerializeField] private LayerMask tileLayer;

    /// <summary>
    /// The distance of the raycast
    /// </summary>
    [SerializeField] private float raycastDistance = 100f;

    /// <summary>
    /// Whether the camera is frozen
    /// </summary>
    [SerializeField] private static bool isFrozen;

    /// <summary>
    /// The camera object
    /// </summary>
    private Camera cam;

    /// <summary>
    /// The last position of the pan
    /// </summary>
    private Vector2 lastPanPosition;

    /// <summary>
    /// Whether the camera is panning
    /// </summary>
    private bool isPanning = false;

    /// <summary>
    /// The player input
    /// </summary>
    private PlayerInput playerInput;

    /// <summary>
    /// The InputAction objects for touch input
    /// </summary>
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    private InputAction touchDeltaAction;

    /// <summary>
    /// When the script is run, set up the camera and input actions
    /// </summary>
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

    /// <summary>
    /// When the script is enabled, add the methods from this class into the touch input events
    /// </summary>
    private void OnEnable()
    {
        touchPressAction.performed += OnTouchPressed;
        touchPressAction.canceled +=  OnTouchReleased;
        Touch.onFingerMove += OnFingerMove;
    }

    /// <summary>
    /// When the script is disabled, remove the methods from this class from the touch input events
    /// </summary>
    private void OnDisable()
    {
        touchPressAction.performed -= OnTouchPressed;
        touchPressAction.canceled -= OnTouchReleased;
        Touch.onFingerMove -= OnFingerMove;
    }

    /// <summary>
    /// Every frame handle the zooming of the camera
    /// </summary>
    private void Update()
    {
        if (cam == null) return;

        HandleZooming();
    }
    
    /// <summary>
    /// When the touch is pressed, start panning the camera
    /// </summary>
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

    /// <summary>
    /// When the finger moves, pan the camera
    /// </summary>
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

    /// <summary>
    /// When the touch is released, stop panning the camera and handle tile selection
    /// </summary>
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

    /// <summary>
    /// Handle the zooming of the camera if there are more than two fingers on the screen
    /// </summary>
    private void HandleZooming()
    {
        if (gameObject.activeSelf && !isFrozen && Touch.activeTouches.Count == 2)
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
    /// <summary>   
    /// Zoom the camera in or out
    /// </summary>
    private void Zoom(float increment)
    {
        if (gameObject.activeSelf)
        {
            if (cam != null && cam.orthographic)
            {
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, minZoom, maxZoom);
            } else if (cam != null)
            {
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - increment, minZoom, maxZoom);
            }
        }
    }
    /// <summary>
    /// Handle the selection of a tile
    /// </summary>
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
                } else
                {
                    Debug.LogWarning("Hit object does not have a Tile component.");
                }
            } else
            {
                isFrozen = false;
                if (Map.Singleton != null && Map.Singleton.isActiveAndEnabled) 
                {
                    Map.Singleton.HideDashedLineAndButton();
                }
                if (Base.Singleton != null && Base.Singleton.isActiveAndEnabled) 
                {
                    Base.Singleton.HideButton();
                }
            }
        }
    }
}
