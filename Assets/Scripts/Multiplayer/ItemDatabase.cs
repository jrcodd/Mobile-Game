using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum ResourceType
{
    Wood,
    Stone,
    Iron,
    Gold,
    Diamond
}

public class ItemDatabase : MonoBehaviour
{
    [Header("ItemSprites")]
    [SerializeField] private Dictionary<string, Sprite> itemSprites = new();
    [SerializeField] public Sprite[] backgroundRollingImages; //0:common 1:uncommon 2:rare 3:epic 4:legendary 5:ancient
    [Header("ItemLists")]
    public List<Item> allItems;
    public List<Item> commonItems;
    public List<Item> uncommonItems;
    public List<Item> rareItems;
    public List<Item> epicItems;
    public List<Item> legendaryItems;
    public List<Item> ancientItems;

    [Header("LootBoxes")]
    public List<LootBox> lootBoxes;
    [SerializeField] private LootBoxUI lootBoxUI;

    private static ItemDatabase _singleton;
    public static ItemDatabase Singleton
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
                Debug.Log($"{nameof(ItemDatabase)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }
    private void OnEnable()
    {
        InitializeItemsLists();
        InitializeLootBoxes();
    }

    void InitializeItemsLists()
    {

        commonItems = new List<Item>
        {
            new Item("Old Nunchaku", 2, Rarity.Common, new List<string>(), "Old Mill", itemSprites["Nunchaku"]),
            new Item("Abandoned Halberd", 7, Rarity.Common, new List<string>(), "Abandoned Hermit’s Hut", itemSprites["Halberd"]),
            new Item("Forgotten Mace", 1, Rarity.Common, new List<string>(), "Forgotten Crypt", itemSprites["Mace"]),
            new Item("Village Warhammer", 8, Rarity.Common, new List<string>(), "Village Well", itemSprites["Warhammer"]),
            new Item("Coastal Greatsword", 3, Rarity.Common, new List<string>(), "Coastal Cave", itemSprites["Greatsword"]),
            new Item("Nomad’s Claymore", 9, Rarity.Common, new List<string>(), "Nomad’s Camp", itemSprites["Claymore"]),
            new Item("Treasure Greatsword", 7, Rarity.Common, new List<string>(), "Treasure Hunter’s Cache", itemSprites["Greatsword"]),
            new Item("Mountain Rapier", 7, Rarity.Common, new List<string>(), "Mountain Pass", itemSprites["Rapier"]),
            new Item("Haunted Spear", 7, Rarity.Common, new List<string>(), "Haunted Mansion", itemSprites["Spear"]),
            new Item("Ancient Quarterstaff", 9, Rarity.Common, new List<string>(), "Ancient Tree Hollow", itemSprites["Quarterstaff"]),
            new Item("Haunted Dagger", 2, Rarity.Common, new List<string>(), "Haunted Mansion", itemSprites["Dagger"]),
            new Item("Farmhouse Falchion", 9, Rarity.Common, new List<string>(), "Farmhouse Attic", itemSprites["Falchion"]),
            new Item("Abandoned Scythe", 5, Rarity.Common, new List<string>(), "Abandoned Mine", itemSprites["Scythe"]),
            new Item("Old Rapier", 8, Rarity.Common, new List<string>(), "Old Forge", itemSprites["Rapier"]),
            new Item("Adventurer’s Dagger", 1, Rarity.Common, new List<string>(), "Adventurer’s Shop", itemSprites["Dagger"]),
            new Item("Mountain Axe", 8, Rarity.Common, new List<string>(), "Mountain Pass", itemSprites["Axe"]),
            new Item("Village Spear", 2, Rarity.Common, new List<string>(), "Village Blacksmith", itemSprites["Spear"]),
            new Item("Ancient Rapier", 4, Rarity.Common, new List<string>(), "Ancient Tree Hollow", itemSprites["Rapier"]),
            new Item("Forgotten Falchion", 3, Rarity.Common, new List<string>(), "Forgotten Crypt", itemSprites["Falchion"]),
            new Item("Coastal Rapier", 10, Rarity.Common, new List<string>(), "Coastal Cave", itemSprites["Rapier"]),
            new Item("Mysterious Mace", 1, Rarity.Common, new List<string>(), "Mysterious Trader", itemSprites["Mace"])
        };

        uncommonItems = new List<Item>
        {
            new Item("Enchanted Greatsword", 13, Rarity.Uncommon, new List<string> { "Frost Nova - Chance to release a burst of frost on hit, slowing all nearby enemiese." }, "Enchanted Forest", itemSprites["Greatsword"]),
            new Item("Forgotten Morning Star", 13, Rarity.Uncommon, new List<string> { "Minor Fear - Small chance to instill fear in enemiese." }, "Forgotten Library", itemSprites["Morning Star"]),
            new Item("Underground Tanto", 11, Rarity.Uncommon, new List<string> { "Berserk Fury - Temporarily increases attack speed after taking damage." }, "Underground Ruins", itemSprites["Tanto"]),
            new Item("Forgotten Katana", 11, Rarity.Uncommon, new List<string> { "Shadow Slash - Increases damage when attacking from behind." }, "Forgotten Bastion", itemSprites["Katana"]),
            new Item("Ancient Mace", 12, Rarity.Uncommon, new List<string> { "Shocking Grasp - Small chance to paralyze enemiese." }, "Ancient Battlefield Relic", itemSprites["Mace"]),
            new Item("Hidden Spear", 13, Rarity.Uncommon, new List<string> { "Siphoning Blow - Slightly restores mana on hit." }, "Hidden Crypt", itemSprites["Spear"]),
            new Item("Dragon’s Mace", 10, Rarity.Uncommon, new List<string> { "Echo Strike - Causes a loud echo that attracts attention." }, "Dragon’s Hoard", itemSprites["Mace"]),
            new Item("Hidden Kusarigama", 10, Rarity.Uncommon, new List<string> { "Soundwave Strike - Small chance to cause a sonic boom." }, "Hidden Valley", itemSprites["Kusarigama"]),
            new Item("Secret Sword", 13, Rarity.Uncommon, new List<string> { "War Cry - Small chance to boost allies’ morale." }, "Secret Hideout", itemSprites["Sword"]),
            new Item("Forgotten Nunchaku", 12, Rarity.Uncommon, new List<string> { "Steady Grip - Slightly improves weapon handling." }, "Forgotten Bastion", itemSprites["Nunchaku"]),
            new Item("Forgotten Quarterstaff", 11, Rarity.Uncommon, new List<string> { "Luminous Blade - Slightly increases light radius in darkness." }, "Forgotten Cathedral", itemSprites["Quarterstaff"]),
            new Item("Forgotten Rapier", 11, Rarity.Uncommon, new List<string> { "Basic Bleed - Small chance to cause minor bleeding." }, "Forgotten Library", itemSprites["Rapier"]),
            new Item("Hidden Halberd", 10, Rarity.Uncommon, new List<string> { "Heated Blade - Slightly increases fire damage." }, "Hidden Tomb", itemSprites["Halberd"]),
            new Item("Cursed Rapier", 10, Rarity.Uncommon, new List<string> { "Life Steal - Heals the user slightly when landing a hit." }, "Cursed Ruins", itemSprites["Rapier"])
        };

        rareItems = new List<Item>
        {
            new Item("Mystic Falchion", 21, Rarity.Rare, new List<string> { "Frost Storm - Summons a storm of ice and snow, freezing enemiese in place." }, "Mystic Maze", itemSprites["Falchion"]),
            new Item("Elemental Kusarigama", 21, Rarity.Rare, new List<string> { "Whirlwind Strike - Allows a spinning attack that hits all nearby enemiese." }, "Elemental Forge", itemSprites["Kusarigama"]),
            new Item("Frozen Claymore", 19, Rarity.Rare, new List<string> { "Glacial Spike - Summons a spike of ice that impales enemiese." }, "Frozen Wasteland", itemSprites["Claymore"]),
            new Item("Lost Katana", 17, Rarity.Rare, new List<string> { "Tidal Wave - Summons a wave of water that pushes enemiese back and deals damage." }, "Lost City", itemSprites["Katana"]),
            new Item("Forgotten Claymore", 15, Rarity.Rare, new List<string> { "Shadowstep - Allows the user to teleport behind an enemy for a powerful strike." }, "Forgotten Sanctuary", itemSprites["Claymore"]),
            new Item("Frozen Trident", 18, Rarity.Rare, new List<string> { "Spiritual Surge - Chance to increase mana regeneration for a short time after a hit." }, "Frozen Wasteland", itemSprites["Trident"]),
            new Item("Lich’s Axe", 20, Rarity.Rare, new List<string> { "Whispering Wind - Slightly increases movement speed after a successful hit." }, "Lich’s Lair", itemSprites["Axe"]),
            new Item("Mystic Kusarigama", 14, Rarity.Rare, new List<string> { "Shadow Slash - Increases damage when attacking from behind." }, "Mystic Spire", itemSprites["Kusarigama"]),
            new Item("Haunted Sword", 17, Rarity.Rare, new List<string> { "Iron Will - Temporarily increases user’s defense after each hit." }, "Haunted Mansion", itemSprites["Sword"])
        };

        epicItems = new List<Item>
        {
            new Item("Elemental Sword", 24, Rarity.Epic, new List<string> { "Steady Grip - Slightly improves weapon handling.", "Iron Will - Temporarily increases user’s defense after each hit." }, "Elemental Plane of Fire", itemSprites["Sword"]),
            new Item("Arcane Spear", 22, Rarity.Epic, new List<string> { "Tectonic Shift - Causes the ground to shift, creating fissures that deal damage and destabilize enemiese.", "Heated Blade - Slightly increases fire damage." }, "Arcane Spire", itemSprites["Spear"]),
            new Item("Elemental Warhammer", 22, Rarity.Epic, new List<string> { "Phoenix Strike - Chance to engulf the weapon in flames, dealing extra fire damage and potentially resurrecting the user if they fall.", "Quick Recovery - Slightly reduces time between attacks." }, "Elemental Plane of Water", itemSprites["Warhammer"]),
            new Item("Feywild Katana", 30, Rarity.Epic, new List<string> { "Toxic Edge - Chance to apply a lingering poison that deals damage over time.", "Toxic Edge - Chance to apply a lingering poison that deals damage over time." }, "Feywild Castle", itemSprites["Katana"]),
            new Item("Mystic Rapier", 24, Rarity.Epic, new List<string> { "Icy Touch - Slightly increases ice damage.", "Inferno Slash - Small chance to set enemiese on fire." }, "Mystic Mountain", itemSprites["Rapier"]),
            new Item("Sky Falchion", 22, Rarity.Epic, new List<string> { "Gale Force - Slightly increases attack speed when outdoors.", "Mystic Barrier - Slight chance to create a temporary shield after a hit." }, "Sky Fortress", itemSprites["Falchion"])
        };

        legendaryItems = new List<Item>
        {
            new Item("Mystic Warhammer", 71, Rarity.Legendary, new List<string> { "Gale Slash - Summons gale-force winds that push enemiese back and increase attack speed greatly.", "Elemental Spark - Small chance to randomly deal elemental damage." }, "Mystic Citadel", itemSprites["Warhammer"]),
            new Item("Celestial Rapier", 77, Rarity.Legendary, new List<string> { "Ethereal Blade - Makes the weapon ethereal, allowing it to pass through armor and deal direct damage to health.", "Weighted Blade - Slightly increases damage to larger enemiese." }, "Celestial Island", itemSprites["Rapier"])
        };

        ancientItems = new List<Item>
        {
            new Item("Kraken’s Flail", 94, Rarity.Ancient, new List<string> { "Void Annihilation - Creates a void that annihilates enemiese, dealing catastrophic damage over time.", "Echoing Strike - Small chance to create a sound distraction.", "Stormcaller’s Fury - Summons a permanent storm that greatly increases lightning damage." }, "Kraken’s Lair", itemSprites["Flail"])
        };
        allItems = commonItems.Concat(uncommonItems).Concat(rareItems).Concat(epicItems).Concat(legendaryItems).Concat(ancientItems).ToList();

    }
    void InitializeLootBoxes()
    {
        lootBoxes = new List<LootBox>();

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

    public Item GetItem(int id)
    {
        if (id >=0 && allItems.Count >id )
        {
            return allItems[id];
        }
        else
        {
            Debug.Log("Item not found");
            return null;
        }
    }
    public int GetIdFromItem(Item item)
    {
        if(allItems.Contains(item))
        {
            return allItems.IndexOf(item);
        }
        else
        {
            Debug.Log("Item not found");
            return -1;
        }
    }
    public void OpenLootBox(PlayerCharacter player, LootBox lootBox, GameObject lootRollCanvas, GameObject mainMenu)
    {
        lootBox.OpenWithAnimation(lootRollCanvas, mainMenu);
    }

    private void Awake()
    {
        // Populate itemSprites dictionary here
        itemSprites.Add("Nunchaku", Resources.Load<Sprite>("Sprites/Nunchaku"));//
        itemSprites.Add("Halberd", Resources.Load<Sprite>("Sprites/Halberd"));//
        itemSprites.Add("Mace", Resources.Load<Sprite>("Sprites/Mace"));//
        itemSprites.Add("Warhammer", Resources.Load<Sprite>("Sprites/Hammer"));//
        itemSprites.Add("Greatsword", Resources.Load<Sprite>("Sprites/Greatsword"));//
        itemSprites.Add("Claymore", Resources.Load<Sprite>("Sprites/Claymore"));//Sword
        itemSprites.Add("Rapier", Resources.Load<Sprite>("Sprites/Rapier"));//
        itemSprites.Add("Spear", Resources.Load<Sprite>("Sprites/Spear"));//
        itemSprites.Add("Quarterstaff", Resources.Load<Sprite>("Sprites/Quarterstaff"));//
        itemSprites.Add("Dagger", Resources.Load<Sprite>("Sprites/Dagger"));//
        itemSprites.Add("Falchion", Resources.Load<Sprite>("Sprites/Falchoin"));//
        itemSprites.Add("Scythe", Resources.Load<Sprite>("Sprites/Scythe"));//
        itemSprites.Add("Axe", Resources.Load<Sprite>("Sprites/Axe"));//
        itemSprites.Add("Morning Star", Resources.Load<Sprite>("Sprites/Morning Star"));//
        itemSprites.Add("Tanto", Resources.Load<Sprite>("Sprites/Tanto"));//
        itemSprites.Add("Katana", Resources.Load<Sprite>("Sprites/Katana"));//
        itemSprites.Add("Kusarigama", Resources.Load<Sprite>("Sprites/Kusarigama"));//Morning Star
        itemSprites.Add("Sword", Resources.Load<Sprite>("Sprites/Sword"));//
        itemSprites.Add("Trident", Resources.Load<Sprite>("Sprites/Trident"));//halberd
        itemSprites.Add("Flail", Resources.Load<Sprite>("Sprites/Flail"));//Nunchaku

        Singleton = this;
    }
    public Color GetColorFromRarity(Rarity rarity)
    {
        //set the color of the item based on its rarity
        switch (rarity)
        {

            case Rarity.Common:
                return new Color(255, 255, 255);

            case Rarity.Uncommon:
                return new Color(0, 100, 0);

            case Rarity.Rare:
                return new Color(0, 0, 100);

            case Rarity.Epic:
                return new Color(100, 0, 100);
                
            case Rarity.Legendary:
                return new Color(255, 130, 0);

            default:
                return new Color(Random.Range(40, 200), Random.Range(40, 100), Random.Range(40, 100));

        }
    }
}
