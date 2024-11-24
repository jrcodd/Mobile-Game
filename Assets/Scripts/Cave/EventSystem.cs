using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventSystem : MonoBehaviour

{
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject rpg;
    [SerializeField] private GameObject floor1Prefab;
    [SerializeField] private GameObject healthBarCanvas;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private GameObject mapCamera;
    [SerializeField] private Vector3 spawnPos = Vector3.zero;


    private GameObject player;
    private GameObject Floor1;

    private GameObject cameraObject;
    private List<GameObject> rpgObjects = new List<GameObject>();




    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerCharacter.Singleton.CheckIfItemIsEquipped();
        if (PlayerCharacter.Singleton.currentLevel < 5)
        {
            print("Player not a high enough level");
            Popup.Singleton.ShowPopup("You must be level 5 to enter the dungeon.\nComplete more tasks to level up!");
            mainMenu.SetActive(true);
            mapCamera.SetActive(true);
            map.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            deathUI.SetActive(false);
            map.SetActive(false);
            mapCamera.SetActive(false);

            cameraObject = Instantiate(cameraPrefab, transform);
            Floor1 = Instantiate(floor1Prefab, transform);
            Player.SetSpawnPos(spawnPos);
            print("Player client id: " + NetworkManager.Singleton.Client.Id);
            UIManager.Singleton.SendName();

            player = Player.GetLocalPlayer();

            foreach (Transform child in rpg.transform)
            {
                rpgObjects.Add(child.gameObject);
            }
        }
    }
  
    public void CompleteDungeon()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        //transfer items and eveything to rpg world
        foreach (GameObject child in gameObjects)
        {
            if(rpgObjects.Contains(child) == false)
            {
                
                
                Destroy(child);

            }
        }
        foreach (GameObject child in gameObjects)
        {
            if (child.GetComponent<Health>() != null)
            {
                Health healthComponent = child.GetComponent<Health>();
                healthComponent.TakeDamage(healthComponent.maxHealth + 1);
            }

        }
        Destroy(cameraObject);
        Destroy(Floor1);
    }
    public void enableDeathUI()
    {
       deathUI.SetActive(true);
       if(player != null)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth.GetHealth() > 0)
            {
                playerHealth.TakeDamage(playerHealth.maxHealth + 1);
            }
        }
        
        CompleteDungeon();

    }
}
