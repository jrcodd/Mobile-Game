using TMPro;
using UnityEngine;

public class BaseButton : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI costText;

    private GameObject next;
    private GameObject current;
    private ResourceType resourceType;
    private int cost;
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

