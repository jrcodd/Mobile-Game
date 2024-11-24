using TMPro;
using UnityEngine;

public class OpenInvButton : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject inventory;
    public TextMeshProUGUI coinsText;
    public GameObject player;

    public void Click()
    {

        mainMenu.SetActive(false);
        inventory.SetActive(true);

    }
    public void OnEnable()
    {
        coinsText.text = player.GetComponent<PlayerCharacter>().GetCoins().ToString();

    }

}
