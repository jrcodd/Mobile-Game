using UnityEngine.AI;
using UnityEngine;

/// <summary>
/// This script is for testing the navMesh movement of the enemy only use for testing
/// </summary>
/// <author>Jackson Codd</author>
/// <version>1.0 Build 2024.11.29</version>
public class testMove : MonoBehaviour
{
    /// <summary>
    /// The player that the enemy will move towards
    /// </summary>
    private Transform player;

    /// <summary>
    /// move towards the player every frame
    /// </summary>
    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;

        }
        if(Time.frameCount % 60 == 0) 
        {
            GetComponent<NavMeshAgent>().destination = (player.position);
        }
    }
}
