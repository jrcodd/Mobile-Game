using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItem : MonoBehaviour
{
    [SerializeField] private MarketItemSlot marketItemSlot;
    public void buyItem()
    {
        Market.Singleton.BuyItem(marketItemSlot.getMarketItem());
    }
}
