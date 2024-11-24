using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstical : MonoBehaviour
{
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, -speed) * Time.deltaTime, Space.World);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Health playerHealthObject = other.gameObject.GetComponent<Health>();
            playerHealthObject.TakeDamage(playerHealthObject.maxHealth/3);
        }
    }
    
}
