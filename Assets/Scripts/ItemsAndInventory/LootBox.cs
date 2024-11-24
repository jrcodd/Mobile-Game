using UnityEngine;
using System.Collections.Generic;
using System.Linq;


// This class represents a loot box in the game
[System.Serializable]
public class LootBox
{
    public readonly string boxName; // The name of the loot box

    private readonly float[] dropChances; // Percentages for item rarities
    private readonly LootBoxUI lootBoxUI; // The UI for the loot box
    private readonly int coinMin; // The minimum number of coins that can be rolled
    private readonly int coinMax; // The maximum number of coins that can be rolled
    private readonly Rarity rarity; // The rarity of the loot box

    public LootBox(string _name, float[] _chances, int[] _coinMinMax, LootBoxUI _lootBoxUI, Rarity _rarity)
    {
        rarity = _rarity;
        lootBoxUI = _lootBoxUI;
        boxName = _name;
        coinMin = _coinMinMax[0];
        coinMax = _coinMinMax[1];    
        dropChances = _chances;
    }

    // Open the loot box
    public (int, List<Item>) Open()
    {
        //returns the number of coins rolled and the item(s) obtained either 1 or 2 items can be pulled 

        //roll coins
        int _coins = Random.Range(coinMin, coinMax + 1);
        List<Item> _possibleDrops;

        //dropChances[0] = 0.8 means 80% common so cutoff would be 800 or less luck = common item
        int _commonItemCuttoff = (int) (dropChances[0] * 1000);
        //dropChances[1] = 0.2 means 20% uncommon cutoff so it would be between 800 and 1000 so 800 + dropChances[1]*1000
        int _unCommonItemCutoff = _commonItemCuttoff + (int) (dropChances[1] * 1000);   
        int _rareItemCutoff = _unCommonItemCutoff + (int) (dropChances[2] * 1000);
        int _epicItemCutoff = _rareItemCutoff + (int) (dropChances[3] * 1000);
        int _legendaryItemCutoff = _epicItemCutoff + (int) (dropChances[4] * 1000);

        List<Item> droppedItems = new();
        int numItems = Random.Range(1, 3);
        //roll item(s) can be 1 or 2 items
        for (int i = 0; i < numItems; i++)
        {
            //luck roll
            int luck = Random.Range(0, 1001);

            if (luck < _commonItemCuttoff)
            {
                _possibleDrops = ItemDatabase.Singleton.commonItems;
            }
            else if (luck < _unCommonItemCutoff)
            {
                _possibleDrops = ItemDatabase.Singleton.uncommonItems;

            }
            else if (luck < _rareItemCutoff)
            {
                _possibleDrops = ItemDatabase.Singleton.rareItems;

            }
            else if (luck < _epicItemCutoff)
            {
                _possibleDrops = ItemDatabase.Singleton.epicItems;

            }
            else if (luck < _legendaryItemCutoff)
            {
                _possibleDrops = ItemDatabase.Singleton.legendaryItems;

            }
            else
            {
                _coins = -2;
                _possibleDrops = ItemDatabase.Singleton.commonItems;    

            }
            Item selectedItem;
            //select a random item from the rolled rarity
            selectedItem = _possibleDrops[Random.Range(0, _possibleDrops.Count)];
            droppedItems.Add(selectedItem);
        }
        return (_coins, droppedItems);
    }

    // Randomize the items so that you get a new one each roll
    List<Item> RandomizeItems(List<Item> itemsList)
    {
        List<Item> items = new();
        items.AddRange(itemsList);
        foreach (Item item in itemsList)
        {
            items.Remove(item);
            items.Insert(Random.Range(0,itemsList.Count()), item);
        }
        return items;
    }
    // Open the loot box with animation
    public (int coins, List<Item> droppedItems) OpenWithAnimation(GameObject lootRollCanvas, GameObject mainMenu)
    {
        // Hide the main menu and show the loot roll canvas
        mainMenu.SetActive(false);
        lootRollCanvas.SetActive(true);

        // Get the coins and items dropped
        (int coins, List<Item> droppedItems) = Open();

        // Get the possible drops
        List< Item > possibleDrops = new();

        // Get the total number of items displayed
        int totalItems = lootBoxUI.displayItemCount;

        //instantiate possibleDrops with the items that can be rolled based on the drop chances
        List<Item>[] itemsByRarity = { ItemDatabase.Singleton.commonItems, ItemDatabase.Singleton.uncommonItems, ItemDatabase.Singleton.rareItems, ItemDatabase.Singleton.epicItems, ItemDatabase.Singleton.legendaryItems };
        for(int i = 0; i < itemsByRarity.Length; i++)
        {
            if (dropChances[i] > 0)
            {
                //make the shown common items proportional to the chance they wil appear so if there are 50 items scrolling then there should be 40 common items if chance[0] = 0.8f
                //while we need to add more...
                for (int j = 0; j < (totalItems * dropChances[i]); j++)
                {
                    //add more
                    possibleDrops.Add(itemsByRarity[i][Random.Range(0, itemsByRarity[i].Count())]);
                }
            }
        }

        //randomize the possible drops
        possibleDrops = RandomizeItems(possibleDrops);

        // Animate the drops
        if (droppedItems.Count == 1)
        {
            lootBoxUI.StartScrollAnimation(possibleDrops, droppedItems[0],  lootRollCanvas,  mainMenu, rarity);
        }
        else if (droppedItems.Count == 2)
        {
            List<Item>[] drops = { RandomizeItems(possibleDrops), RandomizeItems(possibleDrops) };
            lootBoxUI.StartScrollAnimation(drops, droppedItems[0], droppedItems[1],  lootRollCanvas,  mainMenu, rarity);
        }
        // Return the coins and dropped items
        return (coins, droppedItems);
    }
}
