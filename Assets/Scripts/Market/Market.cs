using Riptide;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    [Header("UI")]

    public static List<MarketItem> marketItems = new List<MarketItem>();

    [SerializeField] private Transform itemPanel; // The panel that contains the item slots
    [SerializeField] private GameObject marketItemSlot; // The prefab for the item slot UI
    [SerializeField] private int itemsPerPage = 10; // Items to display per page
    [SerializeField] private TextMeshProUGUI coinsText; // The text displaying the player's coins
    [SerializeField] private float scrollMultiplier = 1.5f; // The multiplier for scrolling speed
    private List<MarketItemSlot> itemSlots = new List<MarketItemSlot>();
    private bool transactionInProgress = false;
    private static Market _singleton;
    public static Market Singleton
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
                print($"{nameof(Market)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }
    private void OnEnable()
    {
        // refreshUI(1);
        getItemsFromServer();
        refreshUI();

    }


    public static void ListItem(int itemId, int price)
    {
        //item id, price
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.listItem);
        message.AddUShort(NetworkManager.Singleton.Client.Id);
        message.AddInt(itemId);
        message.AddInt(price);

        NetworkManager.Singleton.Client.Send(message);
    }
    private void Update()
    {
        HandleScrolling();

    }
    public void BuyItem(MarketItem item)
    {
        if (item != null)
        {
            /* commenting so i can test with only one client
            if(item.seller == NetworkManager.Singleton.Client.Id)
            {
                print("cant buy your own item");
                return;
            }
            */
            if (PlayerCharacter.Singleton.GetCoins() >= item.price)
            {
                transactionInProgress = true;
                //remove item from local enemyConrollerList:
                //send request to server to buy the item
                Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.buyItem);
                //buyer id, seller id, item id, price
                print("sending request to buy the item");
                message.AddUShort(NetworkManager.Singleton.Client.Id);
                message.AddUShort(item.seller);
                message.AddInt(ItemDatabase.Singleton.GetIdFromItem(item.item));
                message.AddInt(item.price);
                NetworkManager.Singleton.Client.Send(message);
            }
            else
            {
                print("not enough coins to buy item");
            }

        }
        else
        {
            print("item is null");
        }
    }
    
    public void refreshUI()
    {
        //send request to server to get items
        getItemsFromServer();
        //update the UI
        coinsText.SetText("Coins: " + PlayerCharacter.Singleton.GetCoins().ToString());
        CreateItemSlots();
        print(itemPanel.position);
        itemPanel.position = new Vector3(itemPanel.position.x, -1266, itemPanel.position.z);
    }
    private void CreateItemSlots()
    {
        foreach (Transform child in itemPanel)
        {
            Destroy(child.gameObject);
        }
        if (marketItems != null)
        {
            itemSlots.Clear();
            for (int i = 0; i < marketItems.Count; i++)
            {
                GameObject slot = Instantiate(marketItemSlot, itemPanel);
                itemSlots.Add(slot.GetComponent<MarketItemSlot>());
                slot.GetComponent<MarketItemSlot>().SetListing(marketItems[i].item, marketItems[i].seller, marketItems[i].price, marketItems[i].listedTime);
            }
        }
    }
    public static void UpdateItemsList(ushort sellerId, int itemId, int price)
    {
        //seller id, item id, price
        if (sellerId == NetworkManager.Singleton.Client.Id)
        {
            //then the client listed the item
            //TODO: make it so you cant buy your own item
        }
        Item item = ItemDatabase.Singleton.GetItem(itemId);
        MarketItem marketItem = new MarketItem(item, price, sellerId, System.DateTime.Now);
        marketItems.Add(marketItem);

    }
    private void HandleScrolling()
    {

        int itemSlotsPerPage = 7;
        int cutoff = 500;
        // this took too long to find but it works now and i can use it fo the other screen
        float resetPos = -Screen.height + 150 + (itemSlots.Count + 1) * (itemPanel.GetComponent<GridLayoutGroup>().spacing.y + itemPanel.GetComponent<GridLayoutGroup>().cellSize.y);
        if (itemSlots.Count > itemSlotsPerPage)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 touchDeltaPosition = touch.deltaPosition;
                    itemPanel.position = new Vector3(itemPanel.position.x, itemPanel.position.y + touchDeltaPosition.y * scrollMultiplier, itemPanel.position.z);
                }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        if (itemSlots[^1].transform.position.y > cutoff)
                        {
                            print(itemSlots.Count);
                            print(itemSlots[^1].transform.position.y);
                            print(itemSlots[^1].name);
                            StartCoroutine(ResetUI(new Vector3(itemPanel.localPosition.x, resetPos, itemPanel.localPosition.z), 0.5f));
                        }
                        else if (itemSlots[0].transform.position.y < Screen.height - cutoff)
                        {
                            StartCoroutine(ResetUI(new Vector3(itemPanel.localPosition.x, 0, itemPanel.localPosition.z), 0.5f));
                        }
                    } 
            }
        }
    }
    private IEnumerator ResetUI(Vector3 endPos, float reboundTime)
    {
        float _elapsedTime = 0f;
        while (_elapsedTime < reboundTime)
        {
            float _t = _elapsedTime / reboundTime;
            float _easedT = ShapingFunctions.Squared(_t);
            itemPanel.localPosition = Vector3.Lerp(itemPanel.localPosition, endPos, _easedT);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    public void getItemsFromServer()
    {
            
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.item);
        //print("sending an item request");
        NetworkManager.Singleton.Client.Send(message);
            
    }
    #region Messages

    [MessageHandler((ushort)ServerToClientId.item)]
    private static void AddItem(Message message) 
    {
        //seller id, item id, price
        //print("getting an item from the server");
        ushort sellerId = message.GetUShort();
        int itemId = message.GetInt();
        int price = message.GetInt();
        UpdateItemsList(sellerId, itemId, price);
        //print("got the item!");
    }

    [MessageHandler((ushort)ServerToClientId.clearMarketList)]
    private static void ClearMarketList(Message message)
    {
        marketItems.Clear();  
    }

    [MessageHandler((ushort)ServerToClientId.itemSold)]
    private static void ItemSold(Message message)
    {
        //buyer id, seller id, item id, price
        ushort sellerId = message.GetUShort();
        ushort buyerId = message.GetUShort();
        int price = message.GetInt();
        int itemId = message.GetInt();
        if (NetworkManager.Singleton.Client.Id == sellerId)
        {
            PlayerCharacter.Singleton.AddCoins(price);
        }
        if(NetworkManager.Singleton.Client.Id == buyerId)
        {
            PlayerCharacter.Singleton.AddCoins(-price);
            PlayerCharacter.Singleton.AddItem(ItemDatabase.Singleton.GetItem(itemId));
        }

        Market.Singleton.transactionInProgress = false;
        //print("transaction complete");
    }

    #endregion
    private void Awake()
    {
        Singleton = this;
    }

}
