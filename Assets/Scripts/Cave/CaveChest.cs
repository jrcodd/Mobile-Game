using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CaveChest : MonoBehaviour
{
    [SerializeField] private int[] possibleBoxes = { 0, 1 }; // The possible loot boxes that can be spawned from the chest (only use 2) (90% chance for the first, 10% for the second)
    void OnTriggerEnter(Collider other)
    {
        // Check if the player collides with the chest
        if (other.CompareTag("Player"))
        {
            // Open the chest
            OpenChest();
        }
    }
    void OpenChest()
    {
        // Play the chest opening animation
        //GetComponent<Animator>().SetTrigger("Open");
        // Disable the collider so the player can't open the chest again
        GetComponent<Collider>().enabled = false;
        // Disable the chest's renderer so the player can't see the chest anymore
        foreach (Transform child in transform)
        {
            child.GetComponent<Collider>().enabled = false;
            // Disable the chest's renderer so the player can't see the chest anymore
        }
        // Spawn the loot
        print("Player opened the chest!");
        LootBox box = Random.value > 0.9f ? ItemDatabase.Singleton.lootBoxes[possibleBoxes[0]] : ItemDatabase.Singleton.lootBoxes[possibleBoxes[1]];
        PlayerCharacter.Singleton.AddLootBox(box);
        print($"Player got a {box.boxName} loot box!");
        Destroy(gameObject);
    }
}
