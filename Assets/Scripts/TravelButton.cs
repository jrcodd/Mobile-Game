using UnityEngine;
using TMPro;

public class TravelButton : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI distanceText;

    private GameObject next;
    private GameObject current;
    private int distance;
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
    public void UpdateButton(GameObject next, GameObject current, int _distance)
    {
        if(_distance < 0) _distance = -_distance;
        distance = _distance;
        distanceText.text = $"{distance} Steps";
        this.next = next;
        this.current = current;
    }
}
