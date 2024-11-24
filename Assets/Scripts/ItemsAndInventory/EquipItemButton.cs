using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItemButton : MonoBehaviour
{
    private Item item;
    public void Click()
    {
        PlayerCharacter.Singleton.EquipItem(item);
    }
}
