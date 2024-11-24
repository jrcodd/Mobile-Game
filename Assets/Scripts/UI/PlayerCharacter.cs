using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerCharacter : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject xpBarObject;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI coinsText;

    private int xp;
    private int coins;
    private float health;
    private float damageMultiplier;
    private List<Item> inventory;
    public int currentLevel;
    private int xpToLevelUp;
    private List<LootBox> lootBoxes;
    public Item equippedItem;
    private int steps;
    public int stamina;
    private Dictionary<ResourceType, int> resources;

    private static PlayerCharacter _singleton;
    public static PlayerCharacter Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                print($"{nameof(PlayerCharacter)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }

    void Start()
    {
        resources = new Dictionary<ResourceType, int>
        {
            { ResourceType.Wood, 0 },
            { ResourceType.Stone, 0 },
            { ResourceType.Iron, 0 },
            { ResourceType.Gold, 0 },
            { ResourceType.Diamond, 0 }
        };

        inventory = new List<Item>();
        lootBoxes = new List<LootBox>();
        xp = 0;
        coins = 0;
    }
    public int GetResource(ResourceType resource)
    {
        return resources[resource];
    }
    public void AddResource(ResourceType resource, int amount)
    {
        resources[resource] += amount;
    }
    public void GetPlayerData()
    {
        PlayerDataSaving.Singleton.GetData();
    }
   
    //update the xp bar with the current xp using maths which is in the google sheet


    //add xp to the player
    public void AddXp(int _amount)
    {
        xp += _amount;
        health = 100 + (currentLevel * 25);
        damageMultiplier = 1 + (currentLevel * 0.025f);
        PlayerDataSaving.Singleton.SaveXp(xp);
    }
    public void AddSteps(int _amount)
    {
        steps += _amount;
        PlayerDataSaving.Singleton.SaveSteps(steps);
    }

    //add coins to the player
    public void AddCoins(int _amount)
    {
        coins += _amount;
        PlayerDataSaving.Singleton.SaveCoins(coins);
        coinsText.text = PlayerCharacter.Singleton.GetCoins().ToString();
        
    }

    //add an item to the player's inventory
    public void AddItem(Item _newItem)
    {
        inventory.Add(_newItem);
        PlayerDataSaving.Singleton.SaveInventory(inventory);
    }

    //getters
    public int GetXp()
    {
        return xp;
    }

    public int GetCoins()
    {
        return coins;
    }
    public int GetSteps()
    {
        return steps;
    }

    public Item GetItem(int _index)
    {
        return inventory[_index];
    }
    public List<LootBox> GetLootBoxes()
    {
        return lootBoxes;
    }
    public List<Item> GetItems()
    {
        return inventory;
    }

    //remove an item from the player's inventory
    public void RemoveItem(Item _item)
    {
        if (inventory.Contains(_item))
        {
            inventory.Remove(_item);
            PlayerDataSaving.Singleton.SaveInventory(inventory);
        }
        else
        {
            print("Item not found in inventory");
        }
    }

    //add a lootbox to the player's lootbox enemyConrollerList
    public void AddLootBox(LootBox _box)
    {
        lootBoxes.Add(_box);
    }

    //remove a lootbox from the player's lootbox enemyConrollerList
    public void RemoveLootBox(int _index = 0)
    {
        lootBoxes.Remove(lootBoxes[_index]); 
    }
    public void EquipItem(Item _item)
    {
        equippedItem = _item;
    }
    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }
    public float GetHealth()
    {
        health = 100 + (currentLevel * 25);
        return health;
    }
    public void CheckIfItemIsEquipped()
    {
        if (equippedItem.damage == 0)
        {
            if(inventory.Count > 0)
            {
                foreach(Item item in inventory)
                {
                    if(item.damage > equippedItem.damage)
                    {
                        equippedItem = item;
                    }
                }
                print($"No item was equipped, so the best item {equippedItem.itemName} was equipped");
            }
        }
    }
    private void Awake()
    {
        Singleton = this;
    }
}
