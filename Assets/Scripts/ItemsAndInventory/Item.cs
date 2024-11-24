using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    Epic = 3,
    Legendary = 4,
    Ancient = 5
}

[System.Serializable]
// This class represents an item in the game
public class Item
{
    public Sprite itemImage; // The image of the item
    public string itemName; // The name of the item
    public int damage; // The damage the item does (not implemented yet)
    public Rarity rarity; // The rarity of the item
    public List<string> abilities; // The abilities of the item (not implemented yet)
    public string locationFound; // The location where the item was found (not implemented yet)

    public Item(string _name, int _dmg, Rarity _rarity, List<string> _abilities, string _location, Sprite _itemImage)
    {
        itemImage = _itemImage;
        itemName = _name;
        damage = _dmg;
        rarity = _rarity;
        abilities = _abilities;
        locationFound = _location;
    }
}
