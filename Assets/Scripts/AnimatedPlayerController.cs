using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPlayerController : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystick; // Reference to the joystick
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject slashAnimation; // Reference to the slash animation prefab
    private void Start()
    {
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FixedJoystick>();
    }
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
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
}
