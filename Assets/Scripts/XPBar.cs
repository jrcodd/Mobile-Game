using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

/// <summary>
/// This script is responsible for updating the xp bar on the ui
/// </summary>
/// <author>Jackson Codd</author>
/// <version>1.0 Build 2024.11.24</version>
public class XPBar : MonoBehaviour
{
    /// <summary>
    /// The game object that the xp bar is attached to
    /// </summary>
    [SerializeField] private GameObject xpBarObject;

    /// <summary>
    /// The text object that displays the level of the player
    /// </summary>
    [SerializeField] private TextMeshProUGUI levelText;

    /// <summary>
    /// The current level of the player
    /// </summary>
    private int currentLevel;

    /// <summary>
    /// The amount of xp needed to level up
    /// </summary>
    private int xpToLevelUp;

    /// <summary>
    /// The amount of xp that the player has
    /// </summary>
    private int xp;

    /// <summary>
    /// The inventory of the player
    /// </summary>
    private List<Item> inventory;

    /// <summary>
    /// instantiate the xp bar and level text
    /// </summary>
    private void OnEnable()
    {
        if (xpBarObject == null)
        {
            xpBarObject = gameObject.GetComponentInChildren<Slider>().gameObject;
        }
        levelText = levelText != null ? levelText : gameObject.GetComponentInChildren<TextMeshProUGUI>();
        InvokeRepeating(nameof(UpdateXpBar), 0.0f, 3.0f);

    }

    /// <summary>
    /// Update the xp bar with the new information
    /// </summary>
    private void UpdateXpBar()
    {
        xp = PlayerCharacter.Singleton.GetXp();
        inventory = PlayerCharacter.Singleton.GetItems();
        Slider xpBar = xpBarObject.GetComponent<Slider>();

        //math to calculate the level based on xp and how much xp is needed to level up and how far the slider should be (based on google sheets equation)
        currentLevel = (int)((-150 + Math.Sqrt(22500 + 600 * xp)) / 300);
        PlayerCharacter.Singleton.currentLevel = currentLevel;

        xpToLevelUp = (150 * (currentLevel + 1) + 150 * ((currentLevel + 1) * (currentLevel + 1))) - (150 * (currentLevel) + 150 * ((currentLevel) * (currentLevel)));

        int totalXpNeeded = (150 * (currentLevel + 1) + 150 * ((currentLevel + 1) * (currentLevel + 1)));

        int currentProgress = xpToLevelUp - (totalXpNeeded - xp);
        xpBar.maxValue = xpToLevelUp;
        xpBar.value = currentProgress;

        levelText.text = "Level " + currentLevel;

        //add a starter item when the player reaches level 5 so they can go into dungeons/bosses
        if (currentLevel >= 5)
        {
            if (inventory.Count == 0)
            {
                Item _item = ItemDatabase.Singleton.GetItem(0);
                PlayerCharacter.Singleton.AddItem(_item);
                PlayerCharacter.Singleton.equippedItem = _item;
            }
        }
    }
}
