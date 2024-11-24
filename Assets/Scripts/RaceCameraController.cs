using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class RaceCameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 normalOffset = new Vector3(0, 7, -20);
    private void Update()
    {
        print("Framerate: " + 1 / Time.deltaTime);
        FollowPlayer();
    }
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
            GoToPlayer(0.2f, desiredPosition);

        }

    }
    private void GoToPlayer(float reboundTime, Vector3 desiredPosition)
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
