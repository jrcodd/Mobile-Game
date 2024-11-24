using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.VFX;

public class RaceFinish : MonoBehaviour
{
    public GameObject deathUI;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI rewardsText;
    public CollectLootBox rewards;
    public Transform[] spawners;
    float score;
    bool isAlive = true;
    public Health health;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (health.GetHealth() <= 0)
            {
                finishRace();
                isAlive = false;
            }
            score += Time.deltaTime*1000;
            scoreText.text = "Score: " + ((int)(score)).ToString();

        }
        
    }
    public void finishRace()
    {
        deathUI.SetActive(true);
        rewards.setLoot((int) score/1000);


    }
    public void resetRace(GameObject player)
    {
        score = 0f;
        isAlive = true;
        deathUI.SetActive(false);
        player.SetActive(true);
        player.GetComponent<Health>().Revive();
        foreach (Transform spawner in spawners)
        {
            foreach (Transform child in spawner)
            {
                Destroy(child.gameObject); 
            }
        }
    }
}
