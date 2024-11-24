using TMPro;
using UnityEngine;

public class CollectLootBox : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI rewardsText;

    //rewards
    private int numCoins;
    private LootBox lootBox;

    //add rewards to player
    private void collectLoot()
    {
        if (lootBox != null) ItemDatabase.Singleton.lootBoxes.Add(lootBox);
        PlayerCharacter.Singleton.AddCoins(numCoins);
    }

    //sets the rewards with an overload to include loot boxes
    public void setLoot(int _coins)
    {
        numCoins = _coins;
        string rewardsString = "+" + numCoins.ToString() + " Coins";
        rewardsText.text = rewardsString;
    }

    public void setLoot(int _coins, LootBox _box)
    {
        numCoins  = _coins;
        lootBox = _box;
        string rewardsString = "+" + numCoins.ToString() + " Coins";
        rewardsString += "\n+1" + " loot box";
    }
}
