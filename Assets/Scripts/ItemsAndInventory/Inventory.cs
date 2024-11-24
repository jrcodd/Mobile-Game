using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static List<LootBox> lootBoxes;
    public LootBoxUI lootBoxUI;
    public List<String> itemTypes;
    public List<Sprite> itemSpritesList;
    public Dictionary<String, Sprite> itemSprites = new Dictionary<String, Sprite>();
    private void Start()
    {
        lootBoxes = new List<LootBox>();
        itemTypes = new List<String>{
            "Sword", "Axe", "Scythe", "Falchion", "Claymore", "Katana",
        "Greatsword", "Trident", "Dagger", "Spear", "Rapier", "Mace", "Warhammer",
        "Flail", "Quarterstaff", "Nunchaku", "Halberd", "Tanto", "Morning Star", "Kusarigama"};


        for (int i = 0; i < itemTypes.Count(); i++)
        {
            itemSprites[itemTypes[i]] = itemSpritesList[i];
        }
        InitializeLootBoxes();
    }
    public void InitializeLootBoxes()
    {
        float[] commonLootChances = { 0.85f, 0.15f, 0f, 0f, 0f };
        int[] commonCoins = { 1, 3 };

        float[] unCommonLootChances = { 0.65f, 0.35f, 0f, 0f, 0f };
        int[] unCommonCoins = { 2, 4 };

        float[] rareLootChances = { 0.55f, 0.35f, 0.1f, 0f, 0f };
        int[] rareCoins = { 3, 5 };

        float[] epicLootChances = { 0f, 0.55f, 0.35f, 0.1f, 0f };
        int[] epicCoins = { 4, 8 };

        float[] legendaryLootChances = { 0f, 0f, 0.25f, 0.75f, 0.05f };
        int[] legendaryCoins = { 7, 15 };

        float[] ancientLootChances = { 0f, 0f, 0f, 0.5f, 0.5f };
        int[] ancientCoins = { 9, 20 };

        List<LootBox> allLootBoxes = new List<LootBox>
        {
            new LootBox("Common LootBox", commonLootChances,commonCoins,lootBoxUI, Rarity.Common),
            new LootBox("Uncommon LootBox", unCommonLootChances,unCommonCoins, lootBoxUI,  Rarity.Uncommon),
            new LootBox("Rare LootBox", rareLootChances, rareCoins,lootBoxUI , Rarity.Rare),
            new LootBox("Epic LootBox",epicLootChances, epicCoins, lootBoxUI, Rarity.Epic),
            new LootBox("Legendary LootBox", legendaryLootChances, legendaryCoins,lootBoxUI, Rarity.Legendary),
            new LootBox("Ancient LootBox", ancientLootChances, ancientCoins, lootBoxUI,  Rarity.Ancient)
        };
        foreach (LootBox lootBox in allLootBoxes)
        {
            lootBoxes.Add(lootBox);
        }
    }

    public void OpenLootBox(PlayerCharacter player, LootBox lootBox, GameObject lootRollCanvas, GameObject mainMenu)
    {
        lootBox.OpenWithAnimation(lootRollCanvas, mainMenu); 
    }
}
