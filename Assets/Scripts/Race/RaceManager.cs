using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [Header("prefabs")]
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject player;

    void OnEnable()
    {
        deathUI.SetActive(false);
        player.GetComponent<RaceFinish>().resetRace(player);

    }


}
