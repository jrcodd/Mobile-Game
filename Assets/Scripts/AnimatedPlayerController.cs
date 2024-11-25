using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///This script is responsible for controlling the player's movement animations
///</summary>
///<author>Jackson Codd</author>
///<version>1.0 Build 2024.11.24</version>
public class AnimatedPlayerController : MonoBehaviour
{
    /// <summary>
    /// Reference to the joystick
    /// </summary>
    [SerializeField] private FixedJoystick joystick;

    /// <summary>
    /// Reference to the animator
    /// </summary>
    [SerializeField] private Animator animator;

    /// <summary>
    /// Reference to the slash animation
    /// </summary>
    [SerializeField] private GameObject slashAnimation;

    /// <summary>
    /// When the script is run, instantiate the joystick
    /// </summary>
    private void Start()
    {
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FixedJoystick>();
    }

    /// <summary>
    /// Every frame I am checking the joystick input and updating the player's rotation and animation state
    /// </summary>
    private void Update()
    {
        float _horizontal = joystick.Horizontal;
        float _vertical = joystick.Vertical;
        Vector3 moveDirection = new Vector3(_horizontal, 0, _vertical).normalized;

        Vector3 currentPosition = transform.position;
        Vector3 positionToLookAt = currentPosition - moveDirection;
        transform.LookAt(positionToLookAt);

        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("isRunning", true);
        } else
        {
            animator.SetBool("isRunning", false);
        }
    }
}
