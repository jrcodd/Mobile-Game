using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObsticals : MonoBehaviour
{
    public GameObject ground;
    public GameObject[] obsticals;
    public float speed;
    public float rate;
    float lastSpawn = 0f;
    float currentTime = 0f;
    public float minx;
    public float maxx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;
        if (Time.time - lastSpawn >= rate) {
            spawnObstical(transform);
            lastSpawn = Time.time;
        }
    }
    void spawnObstical(Transform position)
    {
        GameObject obstical = obsticals[Random.Range(0,obsticals.Length)];
        spawnObstical(position, obstical);
    }
    void spawnObstical(Transform position,  GameObject obstical)
    {
        Instantiate(obstical, position);

    }

}
