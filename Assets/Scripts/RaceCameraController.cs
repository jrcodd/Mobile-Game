using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

///<summary>
/// This script is for the camera that follows the player in the race minigame
///</summary>
///<author>Jackson Codd</author>
///<version>1.0 Build 2024.11.29</version>
public class RaceCameraController : MonoBehaviour
{

    [Header("Target Settings")]
    /// <summary>
    /// The target that the camera will follow
    /// </summary>
    [SerializeField] private Transform target;
    /// <summary>
    /// The offset of the camera
    /// </summary>
    [SerializeField] private Vector3 normalOffset = new Vector3(0, 7, -20);

    /// <summary>
    /// move the camera to the player every frame
    /// </summary>
    private void Update()
    {
        FollowPlayer();
    }

    /// <summary>
    /// Move the camera to the player
    /// </summary>  
    private void FollowPlayer()
    {
        if (target == null)
        {
            FindPlayer();
            return;
        }
        Vector3 desiredPosition = target.position + normalOffset;
        if(transform.position != desiredPosition)
        {
            SnapToPosition(0.2f, desiredPosition);

        }

    }
    /// <summary>
    /// Move the camera to a position with a rebound effect to make it feel more actiony
    /// </summary>
    /// <param name="reboundTime">The time it takes for the camera to rebound</param>
    /// <param name="desiredPosition">The position that the camera will rebound to</param>
    private void SnapToPosition(float reboundTime, Vector3 desiredPosition)
    {
        StartCoroutine(enumerator(reboundTime, desiredPosition));
    }
    private IEnumerator enumerator(float reboundTime, Vector3 desiredPosition)
    {
        float _elapsedTime = 0f;
        while (_elapsedTime < reboundTime)
        {
            float _t = _elapsedTime / reboundTime;
            float _easedT = ShapingFunctions.SquareRoot(_t);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _easedT);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Find and instantiate the player object
    /// </summary>
    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
            transform.LookAt(target);
        }
    }
    
}
