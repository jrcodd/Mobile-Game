using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is for the resource screen. Here the player can "mine" resources to collect them using steps WIP
/// </summary>
/// <author>Jackson Codd</author>
/// <version>1.0 Build 2024.11.29</version>
public class ResourceScreen : MonoBehaviour
{
    /// <summary>
    /// The type of resource that the player is mining
    /// </summary>
    [SerializeField] private ResourceType resourceType;

    /// <summary>
    /// The text that will show how much of the resource there is
    /// </summary>
    [SerializeField] private TMPro.TextMeshProUGUI quantityText;

    /// <summary>
    /// The text that will show the type of resource
    /// </summary>
    [SerializeField] private TMPro.TextMeshProUGUI resourceTypeText;

    /// <summary>
    /// The slider that will show the quantity of the resource
    /// </summary>
    [SerializeField] private Slider quantityBar;

    /// <summary>
    /// The max quantity of the resource
    /// </summary>
    [SerializeField] private int maxQuantity;

    /// <summary>
    /// The current quantity of the resource
    /// </summary>
    private int quantity;

    /// <summary>
    /// Instantiate the quantity bar
    /// </summary>
    private void OnEnable()
    {
        quantity = maxQuantity;
        quantityBar.value = (float)(quantity / maxQuantity);
        quantityBar.maxValue = maxQuantity;

    }

    /// <summary>
    /// Update the resource screen with the current quantity of the resource
    /// </summary>
    /// <param name="_resource">The type of resource that the player is mining</param>
    /// <param name="_quantity">The current quantity of the resource</param>
    public void UpdateResourceScreen(ResourceType _resource, int _quantity)
    {
        resourceType = _resource;
        quantity = _quantity;
        maxQuantity = _quantity;
        quantityText.text = quantity.ToString();
        resourceTypeText.text = resourceType.ToString();
    }

    /// <summary>
    /// Mine the resource and add it to the player's inventory while removing it from the "pile"
    /// </summary>
    /// <param name="amount">The amount of the resource to mine</param>
    public void mineResource(int amount)
    {
        if(quantity - amount > 0)
        {
            quantity -= amount;
            PlayerCharacter.Singleton.AddResource(resourceType, amount);
            quantityBar.value = (float) (quantity / maxQuantity);
            quantityText.text = quantity.ToString();
        }
    }
}
