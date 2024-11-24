using System.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
 
    [Header("UI")]
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private GameObject itemSlotContainerPrefab;
    [SerializeField] private GameObject itemDetailsCanvas;
    [SerializeField] private GameObject InventoryCanvas;

    private GameObject[] itemContainers;
    int pageWidth;

    [Header("Settings")]
    [SerializeField] private int numItemsPerPage = 12;
    [SerializeField] private float scrollMultiplier = 2f;
    private int pageNumber = 1;

    private ItemInventorySlot[,] itemSlots;
    private Item[] inv;

    private static InventoryManager _singleton;
    public static InventoryManager Singleton
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
                Debug.Log($"{nameof(InventoryManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }
    void OnEnable()
    {
        pageWidth = (int) itemSlotContainerPrefab.GetComponent<RectTransform>().rect.width;
        inv = PlayerCharacter.Singleton.GetItems().ToArray();

        // Initialize the item slots array with the number of items per page and the number of physical pages (left middle right)
        itemContainers ??= new GameObject[3];
        itemSlots = new ItemInventorySlot[itemContainers.Length,numItemsPerPage];
        InstantiateContainers();
        UpdateInventoryUI();

    }
    private void InstantiateContainers()
    {
        //left middle right
        foreach (GameObject container in itemContainers)
        {
            Destroy(container);
        }
        itemContainers[0] = Instantiate(itemSlotContainerPrefab, transform);
        itemContainers[0].transform.position = new Vector3(-pageWidth, 0, 0);
        itemContainers[0].SetActive(false); // hide page -1 since there is no items there

        itemContainers[1] = Instantiate(itemSlotContainerPrefab, transform);
        itemContainers[1].transform.position = Vector3.zero; // page 0

        itemContainers[2] = Instantiate(itemSlotContainerPrefab, transform);
        itemContainers[2].transform.position = new Vector3(pageWidth, 0, 0); // page 1 if theres enough items
        if(inv.Length <= numItemsPerPage)
        {
            itemContainers[2].SetActive(false);
        }
    }
    private void Update()
    {
        HandleScroll();
    }

    // Update the inventory UI
    public void UpdateInventoryUI()
    {

        //on start pageNumber = 1 so activePages = -1, 0, 1
        //if we scroll one page to the right pageNumber = 2 so activePages = 0, 1, 2
        int[] activePages = { pageNumber - 2, pageNumber - 1, pageNumber };
        for (int i = 0; i < itemContainers.Length; i++)
        {
            if (activePages[i] < 0 || activePages[i] > PlayerCharacter.Singleton.GetItems().Count / (numItemsPerPage + 1))
            {
                itemContainers[i].SetActive(false);
            }
            else
            {
                itemContainers[i].SetActive(true);
            }
        }
        
        for (int i = 0; i < itemSlots.GetLength(0); i++)
        {
            //the page is active put items in it
            if (itemContainers[i].activeSelf == true)
            {
                for (int j = 0; j < numItemsPerPage; j++)
                {
                    GameObject _slot = itemContainers[i].transform.GetChild(j).gameObject;
                    itemSlots[i, j] = _slot.GetComponent<ItemInventorySlot>();

                    //hope this works lol
                    //if we are just opeing the inv and i = 1 so activePages = -1, 0, 1
                    //so activePages[i] = 0 then check if inv.Length > j
                    if (inv.Length > ((activePages[i] * numItemsPerPage) + j))
                    {
                        //put items[0] in slot[1,0] or slot[activePages[1],j]if activePages[1] = 0
                        
                        Item _itemThatShouldBeHere = inv[ (activePages[i] * numItemsPerPage) + j];
                        itemSlots[i, j].UpdateItemSlot(_itemThatShouldBeHere);

                    }
                    else
                    {
                        itemSlots[i, j].ClearItem();
                    }
                }
            }
        }
    }
    public void ScrollPages(int _deltaPageNum)
    {
        Vector3 endPos;
        if (_deltaPageNum > 0)
        {
            if (pageNumber < 1+ (PlayerCharacter.Singleton.GetItems().Count / (numItemsPerPage +1)))
            {
                pageNumber++;
            }
        }
        else
        {
            if (pageNumber > 1)
            {
                pageNumber --;
            }
        }
        if (pageNumber < 3)
        {
            endPos = new Vector3((pageNumber - 1) * -pageWidth, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            endPos = new Vector3(-pageWidth, transform.localPosition.y, transform.localPosition.z);
        }

        print($"Page number: {pageNumber}");
        StartCoroutine(ResetUI(endPos, 0.5f));
        UpdateInventoryUI();
    }
    IEnumerator ResetUI(Vector3 endPos, float reboundTime)
    {
        float _elapsedTime = 0f;
        while (_elapsedTime < reboundTime)
        {
            float _t = _elapsedTime / reboundTime;
            float _easedT = ShapingFunctions.Squared(_t);
            transform.localPosition = Vector3.Lerp(transform.localPosition, endPos, _easedT);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    public void HandleScroll()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            transform.position = new Vector3(transform.position.x + (touch.deltaPosition.x * scrollMultiplier), transform.position.y, transform.position.z);
            int posToShowNextPage = 0;
            int posToShowPrevPage = (int) (0.5 * pageWidth);
            if (touch.phase == TouchPhase.Ended)
            {
                if (transform.position.x < posToShowNextPage)
                {
                    ScrollPages(1);
                }
                else if (transform.position.x > posToShowPrevPage)
                {
                    ScrollPages(-1);
                }
                else
                {
                    Vector3 endPos;
                    if (pageNumber < 3)
                    {
                        endPos = new Vector3((pageNumber - 1) * -pageWidth, transform.localPosition.y, transform.localPosition.z);
                    }
                    else
                    {
                        endPos = new Vector3(-pageWidth, transform.localPosition.y, transform.localPosition.z);
                    }
                    StartCoroutine(ResetUI(endPos, 0.5f));
                }
            }


        }
    }

    public void ShowItemDetails(Item item)
    {
        itemDetailsCanvas.SetActive(true);
        InventoryCanvas.SetActive(false);
        itemDetailsCanvas.GetComponent<ItemDetails>().SetItem(item);
    }
   

    private void Awake()
    {
        Singleton = this;
    }
}
public static class ShapingFunctions
{
    public static float Squared(float t)
    {
        return (t*t);
    }
    public static float SquareRoot(float t)
    {
        return Mathf.Sqrt(t);   
    }
}
