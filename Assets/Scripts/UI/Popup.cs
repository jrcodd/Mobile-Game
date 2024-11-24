using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private Canvas popupCanvas;
    private static Popup _singleton;
    public static Popup Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                print($"{nameof(Popup)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }
    public void ShowPopup(string _text)
    {
        popupText.text = _text;
        popupCanvas.gameObject.SetActive(true);
    }
    private void Awake()
    {
        Singleton = this;
    }
}
