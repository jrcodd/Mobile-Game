using System;

public class MarketItem
{
    public Item item;
    public int price;
    public ushort seller;
    public System.DateTime listedTime;
    public ushort id;
    public MarketItem(Item item, int price, ushort seller, System.DateTime listedTime)
    {
        this.item = item;
        this.price = price;
        this.seller = seller;
        this.listedTime = listedTime;
        
    }
}
