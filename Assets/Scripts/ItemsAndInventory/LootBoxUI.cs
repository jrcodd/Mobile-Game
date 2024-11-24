using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LootBoxUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform itemContainer; // Container to hold item slots
    [SerializeField] private HorizontalLayoutGroup hlg; // Reference to the Horizontal Layout Group
    [SerializeField] private GameObject inventoryPanel; // Reference to the Inventory Panel
    [SerializeField] private TextMeshProUGUI winningText; // Content object of the ScrollView for the inventory

    [Header("Prefabs")]
    
    [SerializeField] private GameObject itemTextPrefab; // Prefab for the item text in the inventory
    [SerializeField] private GameObject itemPrefab; // Prefab for an item slot

    [Header("Settings")]
    [SerializeField] public int displayItemCount = 10; // Number of unique items to display
    [SerializeField] private float scrollDuration = 5f; // Duration of the scrolling animation
    [SerializeField] private float delayBetweenSpins = 1f; // Delay between two spins if there are two items

    private static LootBoxUI _singleton;
    public static LootBoxUI Singleton
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
                Debug.Log($"{nameof(LootBoxUI)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }
    private List<GameObject> itemSlots;

    void OnEnable()
    {
        itemSlots = new List<GameObject>();
    }

    // Initialize the scroll panel with the item slots
    private void InitializeItemSlots(int _neededSlots)
    {
        // Clear existing item slots by destroying the GameObjects and clearing the enemyConrollerList
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }
        itemSlots.Clear();

        // Create new item slots based on the needed slots
        for (int i = 0; i < _neededSlots; i++)
        {
            GameObject itemSlot = Instantiate(itemPrefab, itemContainer);
            itemSlots.Add(itemSlot);
        }
    }

    // Start the scroll animation with the given items and winning item
    public void StartScrollAnimation(List<Item> _items, Item _winningItem, GameObject _lootRollCanvas, GameObject _mainMenu, Rarity _rarity)
    {
        StartCoroutine(ScrollAnimation(_items, _winningItem, _lootRollCanvas, _mainMenu, _rarity));
    }
    // overload with 2 winning items
    public void StartScrollAnimation(List<Item>[] _items, Item _firstWinningItem, Item _secondWinningItem, GameObject _lootRollCanvas, GameObject _mainMenu, Rarity _rarity)
    {
        StartCoroutine(ScrollAnimation(_items, _firstWinningItem, _secondWinningItem, _lootRollCanvas, _mainMenu, _rarity));
    }

    // the coroutine that handles the scrolling animation
    private IEnumerator ScrollAnimation(List<Item> _items, Item _winningItem, GameObject _lootRollCanvas, GameObject _mainMenu, Rarity _rarity)
    {
        // Reset the winning text
        winningText.text = "";
        // sets the background image of the inventory panel to the corresponding rarity
        inventoryPanel.GetComponent<Image>().sprite = ItemDatabase.Singleton.backgroundRollingImages[(int)_rarity];
        // starts the scrolling animation
        yield return ScrollItems(_items, _winningItem);
        // set the winning text to the item that won
        winningText.text = _winningItem.itemName;
        winningText.color = ItemDatabase.Singleton.GetColorFromRarity(_winningItem.rarity);
        // waits for a delay before going back to the main menu
        yield return new WaitForSeconds(delayBetweenSpins * 3);

        _mainMenu.SetActive(true);
        _lootRollCanvas.SetActive(false);
    }
    
    // the coroutine that handles the scrolling animation but for two items
    private IEnumerator ScrollAnimation(List<Item>[] _items, Item _firstWinningItem, Item _secondWinningItem, GameObject _lootRollCanvas, GameObject _mainMenu, Rarity _rarity)
    {
        // Reset the winning text
        winningText.text = "";
        // sets the background image of the inventory panel to the corresponding rarity
        inventoryPanel.GetComponent<Image>().sprite = ItemDatabase.Singleton.backgroundRollingImages[(int)_rarity];

        // starts the scrolling animation for the first item
        yield return ScrollItems(_items[0], _firstWinningItem);

        // sets the winning text to the item that won
        winningText.text = _firstWinningItem.itemName;
        winningText.color = ItemDatabase.Singleton.GetColorFromRarity(_firstWinningItem.rarity);

        // waits for a delay before starting the second item scroll
        yield return new WaitForSeconds(delayBetweenSpins);

        // starts the scrolling animation for the second item
        yield return ScrollItems(_items[1], _secondWinningItem);

        // sets the winning text to the item that won
        winningText.text = _firstWinningItem.itemName;
        winningText.color = ItemDatabase.Singleton.GetColorFromRarity(_firstWinningItem.rarity);

        // waits for a delay before going back to the main menu
        yield return new WaitForSeconds(delayBetweenSpins*3);

        _mainMenu.SetActive(true);
        _lootRollCanvas.SetActive(false);

    }

    // the coroutine that handles the scrolling animation
    private IEnumerator ScrollItems(List<Item> items, Item winningItem)
    {
        // Reset the position of the item container
        itemContainer.localPosition = Vector3.zero;
        // Calculate how many slots are needed for the scrolling
        int _neededSlots = displayItemCount * 3; // We need enough slots to cover the entire scroll and wrap around
        // Initialize the item slots with the required number of slots
        InitializeItemSlots(_neededSlots);
        // Create a enemyConrollerList of items to scroll through
        List<Item> _scrollingItems = new(items);

        // Repeat the items enemyConrollerList to ensure we have enough items to fill all slots
        while (_scrollingItems.Count < _neededSlots)
        {
            _scrollingItems.AddRange(items);
        }

        // Determine the winning index to land on
        int _winningIndex = _neededSlots / 2; // Winning item should be in the middle of the total slots

        // Insert the winning item at the correct position
        if (_scrollingItems.Count > _winningIndex)
        {
            _scrollingItems[_winningIndex] = winningItem;
        }
        else
        {
            _scrollingItems.Insert(_winningIndex, winningItem);
        }

        // Fill the slots with items
        for (int i = 0; i < itemSlots.Count; i++)
        {
            int _itemIndex = i % _scrollingItems.Count;
            itemSlots[i].GetComponent<ItemSlot>().SetItem(_scrollingItems[_itemIndex]);
        }

        // Calculate the width of each item slot including spacing
        float _itemWidth = itemPrefab.GetComponent<RectTransform>().sizeDelta.x + hlg.spacing;

        // Calculate the start and end positions for the scroll
        Vector3 _startPosition = itemContainer.localPosition;
        float _centeringOffset = itemPrefab.GetComponent<RectTransform>().sizeDelta.x / 1.25f; // makes sure the item lands in the center kind of jank but it works for 50 4 1 in settings and 1000 spacing and middle left chld allignment
        Vector3 _endPosition = _startPosition - new Vector3(_itemWidth * (_winningIndex) - (_itemWidth / 2) + (_centeringOffset), 0, 0);

        // Animate the scrolling
        float _elapsedTime = 0f;
        while (_elapsedTime < scrollDuration)
        {
            //maths i dont understand lol but it works
            //update: watch this if u dont fw lerping https://www.youtube.com/watch?v=YJB1QnEmlTs
            float _t = _elapsedTime / scrollDuration;
            float _easedT = EasingFunctions.Decelerate(_t);
            itemContainer.localPosition = Vector3.Lerp(_startPosition, _endPosition, _easedT);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the winning item is centered at the end
        itemContainer.localPosition = _endPosition;
        // Set the winning item at the end of the animation
        itemSlots[displayItemCount / 2].GetComponent<ItemSlot>().SetItem(winningItem);

        // Update the inventory panel with the winning item
        AddItemToInventory(winningItem);
    }

    // Add the winning item to the inventory
    private void AddItemToInventory(Item winningItem)
    {
        PlayerCharacter.Singleton.AddItem(winningItem);
    }
    private void Awake()
    {
        Singleton = this;
    }
}

//function for decelerating the scroll
public static class EasingFunctions
{
    public static float Decelerate(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3);
    }
}

