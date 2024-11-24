using UnityEngine;

public class StarRating : MonoBehaviour
{
    [SerializeField] private Star[] rarityStars; // The stars that represent the rarity of the item
    [SerializeField] private int rarity = -1;
    private void OnEnable()
    {
        if(rarity != -1)
        {
            setRarity(rarity);
        }
    }
    public void setRarity(int _rarity)
    {
        for (int i = 0; i < rarityStars.Length; i++)
        {
            if (_rarity >= i)
            {
                rarityStars[i].IsFilled = true;
            }
            else
            {
                rarityStars[i].IsFilled = false;
            }
        }
    }
}
