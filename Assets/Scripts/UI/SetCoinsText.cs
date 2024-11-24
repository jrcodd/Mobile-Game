using TMPro;
using UnityEngine;

public class SetCoinsText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;

    public void OnEnable()
    {
        coinsText.text = PlayerCharacter.Singleton.GetCoins().ToString();
    }

}
