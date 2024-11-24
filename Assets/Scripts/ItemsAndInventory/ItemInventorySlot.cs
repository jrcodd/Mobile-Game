using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventorySlot : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI itemText; // The text that displays the item name
    [SerializeField] private Image itemImage; // The image of the item in the inventory slot
    private Item item; // The item in the slot


    // Update the item slot with the item
    public void UpdateItemSlot(Item _item)
    {

        Color _itemColor = ItemDatabase.Singleton.GetColorFromRarity(_item.rarity);
        item = _item;

        itemImage.enabled = true;
        itemImage.sprite = _item.itemImage;
        itemImage.color = _itemColor;
        //test to see if it looks better with no text I think it looks cleaner
        itemText.gameObject.SetActive(false);
        itemText.text = _item.itemName;
    }
    public void ClearItem()
    {
        itemImage.enabled = false;
        itemText.gameObject.SetActive(false);
    }
    // Show the item details
    public void ShowItemDetails()
    {
        if (item != null)
        {
            InventoryManager.Singleton.ShowItemDetails(item);
            
        }
    }


}
