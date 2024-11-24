using UnityEngine;
using UnityEngine.UI;

public class ResourceScreen : MonoBehaviour
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private TMPro.TextMeshProUGUI quantityText;
    [SerializeField] private TMPro.TextMeshProUGUI resourceTypeText;
    [SerializeField] private Slider quantityBar;
    [SerializeField] private int maxQuantity;
    private int quantity;


    private void OnEnable()
    {
        quantity = maxQuantity;
        quantityBar.value = (float)(quantity / maxQuantity);
        quantityBar.maxValue = maxQuantity;

    }
    public void UpdateResourceScreen(ResourceType _resource, int _quantity)
    {
        resourceType = _resource;
        quantity = _quantity;
        maxQuantity = _quantity;
        quantityText.text = quantity.ToString();
        resourceTypeText.text = resourceType.ToString();
    }
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
