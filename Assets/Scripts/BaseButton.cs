using TMPro;
using UnityEngine;

/// <summary>
/// This script is for the base button on the map. The player needs to have enough of a certain resource to travel here. 
/// </summary>
/// <author>Jackson Codd</author>
/// <version>1.0 Build 2024.11.24</version>
public class BaseButton : MonoBehaviour
{

    [Header("UI")]
    /// <summary>
    /// The text that will show the cost of the travel
    /// </summary>
    [SerializeField] private TextMeshProUGUI costText;

    /// <summary>
    /// The next location that the player will travel to
    /// </summary>
    private GameObject next;

    /// <summary>
    /// The current location that the player is at
    /// </summary>
    private GameObject current;

    /// <summary>
    /// The type of resource that the player needs to have to travel here
    /// </summary>
    private ResourceType resourceType;

    /// <summary>
    /// The cost of the travel
    /// </summary>
    private int cost;

    /// <summary>
    /// When the button is clicked, check if the player has enough resources to travel here
    /// </summary>
    public void Click()
    {
        if (PlayerCharacter.Singleton.GetResource(resourceType) < cost)
        {
            Popup.Singleton.ShowPopup($"You do not have enough {resourceType} to travel here.");
            return;
        }
        PlayerCharacter.Singleton.AddResource(resourceType, -cost);
        current.SetActive(false);
        next.SetActive(true);
    }

    /// <summary>
    /// Update the button with the new information
    /// </summary>
    public void UpdateButton(GameObject _next, GameObject _current, ResourceType _resource, int _cost)
    {
        if (_cost < 0) _cost = -_cost;
        cost = _cost;
        resourceType = _resource;
        costText.text = $"{cost} {_resource}";
        next = _next;
        current = _current;
    }
}

