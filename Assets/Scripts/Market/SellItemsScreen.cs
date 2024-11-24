using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellItemsScreen : MonoBehaviour
{
    private List<ListItemSlot> itemSlots;
    [Header("UI")]
    [SerializeField] private ListItemSlot itemSlotPrefab;
    [SerializeField] private Transform itemSlotContainer;
    private Vector3 startPos;
    [Header("Settings")]
    [SerializeField] private float scrollMultiplier = 1.5f;

    private static SellItemsScreen _singleton;

    public static SellItemsScreen Singleton
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
                print($"{nameof(SellItemsScreen)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }
    private void OnEnable()
    {
        itemSlots = new List<ListItemSlot>();
        UpdateUI();
    }
    private void Update()
    {
        HandleScrolling();
    }
    public void UpdateUI()
    {
        itemSlots.Clear();
        foreach(Transform child in itemSlotContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Item i in PlayerCharacter.Singleton.GetItems())
        {
            ListItemSlot slot = Instantiate(itemSlotPrefab, itemSlotContainer);
            slot.SetItem(i,15*(int)i.rarity, System.DateTime.Now);
            itemSlots.Add(slot);
        }
    }
    private void HandleScrolling()
    {

        int itemSlotsPerPage = 7;
        int cutoff = 500;
        // this took too long to find but it works now and i can use it fo the other screen
        float resetPos = -Screen.height + 150 + (itemSlots.Count + 1) * (itemSlotContainer.GetComponent<GridLayoutGroup>().spacing.y + itemSlotContainer.GetComponent<GridLayoutGroup>().cellSize.y);
        if (itemSlots.Count > itemSlotsPerPage)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 touchDeltaPosition = touch.deltaPosition;
                    itemSlotContainer.position = new Vector3(itemSlotContainer.position.x, itemSlotContainer.position.y + touchDeltaPosition.y * scrollMultiplier, itemSlotContainer.position.z);
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    print(itemSlotContainer.localPosition.y);
                    if (itemSlots[^1].transform.position.y > cutoff)
                    {
                        StartCoroutine(ResetUI(new Vector3(itemSlotContainer.localPosition.x, resetPos, itemSlotContainer.localPosition.z), 0.5f));
                    }
                    else if (itemSlots[0].transform.position.y < Screen.height - cutoff)
                    {
                        StartCoroutine(ResetUI(new Vector3(itemSlotContainer.localPosition.x, 0, itemSlotContainer.localPosition.z), 0.5f));
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
            itemSlotContainer.localPosition = Vector3.Lerp(itemSlotContainer.localPosition, endPos, _easedT);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    private void Awake()
    {
        Singleton = this;
    }
}
