using System.Collections.Generic;
using UnityEngine;

public class LootBoxButton : MonoBehaviour
{
    [SerializeField] private GameObject lootRollCanvas;
    [SerializeField] private GameObject mainMenu;

    private PlayerCharacter player;
    private List<LootBox> lootBoxes;

    private void OnEnable()
    {
        if (PlayerCharacter.Singleton.GetLootBoxes().Count < 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    //open a loot box if the player has one
    public void Click()
    {
        lootBoxes = PlayerCharacter.Singleton.GetLootBoxes();
        player = PlayerCharacter.Singleton;
        if (lootBoxes.Count > 0)
        {
            LootBox loot = lootBoxes[0];
            ItemDatabase.Singleton.OpenLootBox(player, loot, lootRollCanvas, mainMenu);
            player.RemoveLootBox();
        }

        else
        {
            gameObject.SetActive(false);
            Popup.Singleton.ShowPopup("no loot boxes");
        }
    }
}
