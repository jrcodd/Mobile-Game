using UnityEngine.AI;
using UnityEngine;

public class testMove : MonoBehaviour
{
    private Transform player;

    private void OnEnable()
    {

    }
    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;

        }
        if(Time.frameCount % 60 == 0) GetComponent<NavMeshAgent>().destination = (player.position);


    }
}
