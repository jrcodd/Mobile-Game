using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is for the boss enemy. The boss will drop a loot box and coins when defeated.
/// </summary>
/// <author>Jackson Codd</author>
/// <version>1.0 Build 2024.11.24</version>
public class Boss : MonoBehaviour
{
    /// <summary>
    /// The value that will be used in the weight function
    /// </summary>
    [SerializeField] private float floatVal = 0.5f;

    /// <summary>
    /// When the boss is defeated, give the player rewards
    /// </summary>
    public (LootBox, int) GiveRewards()
    {
        float[] chances = GetDropChances(ItemDatabase.Singleton.lootBoxes.Count);
        LootBox bossBox;
        int luck = Random.Range(0, 100);
        int numCoins = luck / 2;
        bossBox = ItemDatabase.Singleton.lootBoxes[Choose(chances)];
        return (bossBox, numCoins);
    }

    /// <summary>
    /// The weight function that will be used to determine the drops
    /// </summary>
    private float WeightFunction(int x)
    {
        return 5 * Mathf.Pow(x + floatVal * 5, 2 * (floatVal - 0.5f));
    }

    /// <summary>
    /// Get the drop chances for the loot boxes
    /// </summary>
    private float[] GetDropChances(int numItems)
    {
        float[] rawChances = new float[numItems];
        float sum = 0;

        for (int i = 0; i < numItems; i++)
        {
            rawChances[i] = WeightFunction(i);
            sum += rawChances[i];
        }
        float[] chances = new float[numItems];
       
        for (int i = 0; i < numItems; i++)
        {
            chances[i] = rawChances[i] / sum;
        }
        return chances;
    }

    /// <summary>
    /// Choose a random item based on the probabilities
    /// </summary>
    private int Choose(float[] probabilities)
    {
        float r = Random.value;
        float cumulativeProbability = 0f;

        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (r <= cumulativeProbability)
            {
                return i;
            }
        }

        // In case of rounding errors or if probabilities don't sum to 1
        return 0;
    }
}
