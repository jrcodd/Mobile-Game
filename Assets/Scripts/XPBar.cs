using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class XPBar : MonoBehaviour
{
    [SerializeField] private GameObject xpBarObject;
    [SerializeField] private TextMeshProUGUI levelText;
    private int currentLevel;
    private int xpToLevelUp;
    private int xp;
    private List<Item> inventory;
    private void OnEnable()
    {
        if (xpBarObject == null)
        {
            xpBarObject = gameObject.GetComponentInChildren<Slider>().gameObject;
        }
        levelText = levelText != null ? levelText : gameObject.GetComponentInChildren<TextMeshProUGUI>();
        InvokeRepeating(nameof(UpdateXpBar), 0.0f, 3.0f);

    }

    private void UpdateXpBar()
    {
        xp = PlayerCharacter.Singleton.GetXp();
        inventory = PlayerCharacter.Singleton.GetItems();
        Slider xpBar = xpBarObject.GetComponent<Slider>();

        //math to calculate the level based on xp
        currentLevel = (int)((-150 + Math.Sqrt(22500 + 600 * xp)) / 300);
        PlayerCharacter.Singleton.currentLevel = currentLevel;
        //math to calculate how much xp is needed to level up
        xpToLevelUp = (150 * (currentLevel + 1) + 150 * ((currentLevel + 1) * (currentLevel + 1))) - (150 * (currentLevel) + 150 * ((currentLevel) * (currentLevel)));

        //calculate how far the slider should be 
        int totalXpNeeded = (150 * (currentLevel + 1) + 150 * ((currentLevel + 1) * (currentLevel + 1)));
        int currentProgress = xpToLevelUp - (totalXpNeeded - xp);
        xpBar.maxValue = xpToLevelUp;
        xpBar.value = currentProgress;

        levelText.text = "Level " + currentLevel;
        if (currentLevel >= 5)
        {
            //add a starter item when the player reaches level 5
            if (inventory.Count == 0)
            {
                Item _item = ItemDatabase.Singleton.GetItem(0);
                PlayerCharacter.Singleton.AddItem(_item);
                PlayerCharacter.Singleton.equippedItem = _item;
            }
        }
    }
}
