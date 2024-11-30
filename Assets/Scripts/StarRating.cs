using UnityEngine;

///<summary>
/// The script for the group of 5 stars for items
/// </summary>
///<author>Jackson Codd</author>
///<version>1.0 Build 2024.11.29</version>
public class StarRating : MonoBehaviour
{
    ///<summary>
    /// The stars that will be filled in
    ///</summary>
    [SerializeField] private Star[] rarityStars; 

    ///<summary>
    /// The rarity of the item
    ///</summary>
    [SerializeField] private int rarity = -1;

    ///<summary>
    /// show the correct number of stars filled in when the object is enabled
    ///</summary>
    private void OnEnable()
    {
        if(rarity != -1)
        {
            setRarity(rarity);
        }
    }

    ///<summary>
    /// fill in the stars up to the rarity of the item so 4 rarity has 4 filled in stars
    ///</summary>
    ///<param name="_rarity">The number of stars to fill in</param>
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
