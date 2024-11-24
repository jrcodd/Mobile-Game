using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ListItemButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField priceText;
    private Item item;

    public void SetItem(Item _item)
    {
        item = _item;
    }

    public void ListItem()
    {
        if (ItemDatabase.Singleton.GetIdFromItem(item) > 1)
        {
            int price = priceText.textComponent.text == "" ? 0 : int.Parse(priceText.text);
            PlayerCharacter.Singleton.RemoveItem(item);
            int itemId = ItemDatabase.Singleton.GetIdFromItem(item);
            Market.ListItem(itemId, price);
            SellItemsScreen.Singleton.UpdateUI();
            print("Listing item");
        }
        else
        {
            Popup.Singleton.ShowPopup("Sorry, You can't enemyConrollerList this item!");
        }
    }
}
