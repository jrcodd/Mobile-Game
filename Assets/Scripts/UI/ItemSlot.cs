using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour
{

    [SerializeField] private Image itemImage; //place where the image will display
    [SerializeField] private TextMeshProUGUI itemName; //place where the name will display

    //attach an item to the item slot
    //(required otherwise there will be an error when you try and dispay the slot)
    public void SetItem(Item item)
    {
        //set the image and name of the item
        itemImage.sprite = item.itemImage;
        itemName.text = item.itemName;
        itemImage.color = ItemDatabase.Singleton.GetColorFromRarity(item.rarity);
    }
}