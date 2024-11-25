using Riptide;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TextMeshProUGUI connectionStatus;
    [SerializeField] private GameObject wbPopup;

    private static UIManager _singleton;
    public static UIManager Singleton
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
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }

    public void ConnectClicked()
    {
        usernameField.interactable = false;
        string ip = usernameField.text;
        NetworkManager.Singleton.SetIp(ip);
        connectionStatus.text = "Connecting...";

        NetworkManager.Singleton.Connect();
    }

    //go into the main menu if we connect to the server
    public void ConnectionSuccess()
    {
        wbPopup.SetActive(true);
        StepTracker.Singleton.GetSteps();
        connectUI.SetActive(false);
    }

    //if we dont connect go back to the connect screen
    public void BackToMain()
    {
        usernameField.interactable = true;
        connectionStatus.text = "Connection Failed";
    }

    //send the username entered and the device id to the server
    public void SendName()
    {
        Message _message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.name);
        string _deviceId = SystemInfo.deviceUniqueIdentifier;
        _message.AddString(usernameField.text);
        _message.AddString(_deviceId);
        _message.AddFloat(PlayerCharacter.Singleton.equippedItem.damage);
        NetworkManager.Singleton.Client.Send(_message);
    }

    private void Awake()
    {
        Singleton = this;
    }
}



