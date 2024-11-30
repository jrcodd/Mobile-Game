using UnityEngine;
using TMPro;

/// <summary>
/// This script is for the travel button on the map. The player needs to have enough steps to travel to the next location.
/// </summary>
/// <author>Jackson Codd</author>
/// <version>1.0 Build 2024.11.24</version>
public class TravelButton : MonoBehaviour
{
    [Header("UI")]

    /// <summary>
    /// The text that will show the cost of the travel
    /// </summary>
    [SerializeField] private TextMeshProUGUI distanceText;

    /// <summary>
    /// The next location that the player will travel to
    /// </summary>
    private GameObject next;
    
    /// <summary>  
    /// The current location that the player is at
    /// </summary>
    private GameObject current;

    /// <summary>
    /// The cost of the travel
    /// </summary>
    private int distance;

    /// <summary>
    /// When the button is clicked, check if the player has enough steps to travel here
    /// </summary>
    public void Click()
    {
        if(PlayerCharacter.Singleton.GetSteps() < distance)
        {
            print("You do not have enough steps to travel here.");
            Popup.Singleton.ShowPopup("You do not have enough steps to travel here.");
            return;
        }
        PlayerCharacter.Singleton.AddSteps(-distance);
        current.SetActive(false);
        next.SetActive(true);
    }

    /// <summary>
    /// Update the button with the new information
    /// </summary>
    /// <param name="next">The next location that the player will travel to</param>
    /// <param name="current">The current location that the player is at</param>
    /// <param name="_distance">The cost of the travel</param>
    public void UpdateButton(GameObject next, GameObject current, int _distance)
    {
        if(_distance < 0) _distance = -_distance;
        distance = _distance;
        distanceText.text = $"{distance} Steps";
        this.next = next;
        this.current = current;
    }
}
