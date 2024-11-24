using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListItemSlot : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI previewText;
    [SerializeField] private Image itemImage;
    [SerializeField] private ListItemButton listItemButton;

    public void SetItem(Item item, int price, System.DateTime listedTime)
    {
        previewText.text = item.itemName + "\n" + price.ToString("F2");
        itemImage.sprite = item.itemImage;
        listItemButton.SetItem(item);
    }
}


