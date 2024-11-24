using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Renderer rendererr;
    public GameObject healthBarPrefab;
    public float maxHealth;
    private float currentHealth;
    private Slider healthBar;
    private Transform healthBarTransform;
    private Canvas canvas;
    private GameObject healthBarObject;
    public bool isBoss = false;
    Color originalColor;
    void OnEnable()
    {
        if (rendererr == null)
        {
            rendererr = GetComponent<Renderer>();

        }
        if (gameObject.CompareTag("Player"))
        {
            if (PlayerCharacter.Singleton != null)
            {
                maxHealth = PlayerCharacter.Singleton.GetHealth();
                print(maxHealth);
            }
            else
            {
                maxHealth = 10;
            }
        }
        currentHealth = maxHealth;
        originalColor = rendererr.material.color;

        // Find the Canvas in the scene
        canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene.");
            return;
        }

    }
    private void UpdateHealthBar()
    {
        // Instantiate the health bar prefab
        if (healthBarObject == null)
        {
            healthBarObject = Instantiate(healthBarPrefab, canvas.transform);
        }
        healthBar = healthBarObject.GetComponent<Slider>();
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        healthBarTransform = healthBarObject.transform;

        // Position the health bar above the character

    }

    void Update()
    {
        // Update the health bar position every frame
        PositionHealthBar();
    }
    public void SetHealth(float health)
    {

            currentHealth = health;
            UpdateHealthBar();
            CheckDeath(true);
        
    }
    private void DropBossLoot()
    {
        LootBox box;
        int coins;
        (box, coins) = (gameObject.GetComponent<Boss>().GiveRewards());
        PlayerCharacter.Singleton.AddLootBox(box);
        PlayerCharacter.Singleton.AddCoins(coins);
    }
    private void DropEnemyLoot()
    {
        if (Random.value < 0.01f)
        {
            Item commonItem = ItemDatabase.Singleton.commonItems[Random.Range(0, ItemDatabase.Singleton.commonItems.Count)];
            print(commonItem.itemName + " dropped!");
            PlayerCharacter.Singleton.AddItem(commonItem);
        }
    }
    private void CheckDeath(bool hit)
    {
        if (currentHealth <= 0)
        {
            Destroy(healthBarObject);
            if (gameObject.CompareTag("Enemy") || gameObject.CompareTag("Boss"))
            {
                if (isBoss && hit)
                {
                        DropBossLoot();
                }
                else
                {
                    if (hit)
                    {
                        DropEnemyLoot();
                    }
                }

            }
            //this is only still here bc i dont want to fw the race mode rn
            else if (gameObject.CompareTag("Player"))
            {
                //destroy the opject on death
                if (gameObject.GetComponent<RaceFinish>() != null)
                {
                    gameObject.GetComponent<RaceFinish>().finishRace();
                    rendererr.material.color = originalColor;
                    gameObject.SetActive(false);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>().enableDeathUI();
                }
            }
            
        }
    }
    public void SetMaxHealth(float health)
    {
        maxHealth = health;
    }

    public void TakeDamage(float damage, bool hit = false)
    {
        if (healthBar != null)
        {
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;
            healthBar.value = currentHealth;
            StartCoroutine(FlashRed());
            //CheckDeath(hit);
            
        }
    }

    private IEnumerator FlashRed()
    {
        if(rendererr == null)
        {
            rendererr = GetComponent<Renderer>();

        }
        rendererr.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rendererr.material.color = originalColor;



    }

    private void PositionHealthBar()
    {
        if (healthBarTransform != null)
        {
            // Convert world position to screen position
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f);
            healthBarTransform.position = screenPosition;
        }
    }
    public float GetHealth()
    {
        return currentHealth;
    }
    public void Revive()
    {

        OnEnable();
        healthBarObject.SetActive(true);
        gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        if (healthBarObject != null)
        {
            Destroy(healthBarObject);
        }
    }
    
}
