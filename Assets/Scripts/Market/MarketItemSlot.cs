using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketItemSlot : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI previewText;
    [SerializeField] private Image itemImage;
    private MarketItem marketItem;
    
    public void SetListing(Item item, ushort seller, int price, System.DateTime listedTime)
    {
        previewText.text = item.itemName + "\n"+ price.ToString("F2");
        itemImage.sprite = item.itemImage;
        marketItem = new MarketItem(item, price, seller, listedTime);
    }
    public MarketItem getMarketItem()
    {
        return marketItem;
    }

}
