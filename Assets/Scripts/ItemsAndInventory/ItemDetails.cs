using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetails : MonoBehaviour
{
    private Item item;
    [Header("UI")]
    [SerializeField] private Image itemRarityImage; // The background image of the item detail canvas
    [SerializeField] private Image itemImage; // The image of the item in the item detail canvas
    [SerializeField] private TextMeshProUGUI itemNameText; // Item Name
    [SerializeField] private TextMeshProUGUI itemDamageText; // Item Damage
    [SerializeField] private TextMeshProUGUI itemAbilitiesText; // Item Abilities
    [SerializeField] private TextMeshProUGUI itemLongDescription; // Item Long Description
    [SerializeField] private StarRating starRating;
    public void SetItem(Item _item)
    {
        item = _item;
        itemRarityImage.sprite = ItemDatabase.Singleton.backgroundRollingImages[(int)item.rarity];
        itemNameText.text = item.itemName;
        starRating.setRarity((int) item.rarity);
        itemImage.sprite = item.itemImage;
        itemDamageText.text = $"{item.damage} Damage";
        itemAbilitiesText.text = string.Join(", ", item.abilities);
        itemLongDescription.text = $"{item.itemName} was found in {item.locationFound}. It has {item.abilities.Count} abilities and a rarity of {item.rarity}"; }
    public void EquipItem()
    {
        PlayerCharacter.Singleton.EquipItem(item);
    }
}
