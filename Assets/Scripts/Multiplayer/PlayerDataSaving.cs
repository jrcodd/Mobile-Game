using System.Collections.Generic;
using UnityEngine;
using Riptide;


public class PlayerDataSaving : MonoBehaviour
{
    private static PlayerDataSaving _singleton;
    public static PlayerDataSaving Singleton
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
                Debug.Log($"{nameof(PlayerDataSaving)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }
    public void SaveInventory(List<Item> _items)
    {
        print($"Saving {_items.Count} items");
        bool _firstItem = true;
        foreach (Item _item in _items)
        {
            int _itemId = ItemDatabase.Singleton.GetIdFromItem(_item);
            Message _message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.saveInventory);
            string _deviceId = SystemInfo.deviceUniqueIdentifier;
            _message.AddString(_deviceId);
            _message.AddInt(_itemId);
            _message.AddBool(_firstItem);
            NetworkManager.Singleton.Client.Send(_message);
            _firstItem = false;
        }
    }
    public void SaveCoins(int _coins)
    {
        print($"Saving {_coins} coins");
        Message _message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.saveCoins);
        string _deviceId = SystemInfo.deviceUniqueIdentifier;
        _message.AddString(_deviceId);
        _message.AddInt(_coins);
        NetworkManager.Singleton.Client.Send(_message);
    }
    public void SaveXp(int _xp)
    {
        print($"Saving {_xp} xp");
        Message _message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.saveXp);
        string _deviceId = SystemInfo.deviceUniqueIdentifier;
        _message.AddString(_deviceId);
        _message.AddInt(_xp);
        NetworkManager.Singleton.Client.Send(_message);
    }
    public void SaveSteps(int _steps)
    {
        print($"Saving {_steps} steps");
        Message _message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.saveSteps);
        string _deviceId = SystemInfo.deviceUniqueIdentifier;
        _message.AddString(_deviceId);
        _message.AddInt(_steps);
        NetworkManager.Singleton.Client.Send(_message);
    }
    public void GetData()
    {
        string _deviceId = SystemInfo.deviceUniqueIdentifier;
        Message _message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.getPlayerData);
        _message.AddString(_deviceId);
        print(NetworkManager.Singleton.Client == null);

        NetworkManager.Singleton.Client.Send(_message);
    }
    

    #region Messages
    [MessageHandler((ushort)ServerToClientId.sendInventory)]
    public static void AddItem(Message message)
    {
        PlayerCharacter.Singleton.AddItem(ItemDatabase.Singleton.GetItem(message.GetInt()));
    }
    [MessageHandler((ushort)ServerToClientId.sendCoins)]
    public static void AddCoins(Message message)
    {
        PlayerCharacter.Singleton.AddCoins(message.GetInt());
    }
    [MessageHandler((ushort)ServerToClientId.sendXp)]
    public static void AddXp(Message message)
    {
        PlayerCharacter.Singleton.AddXp(message.GetInt());
    }
    [MessageHandler((ushort)ServerToClientId.sendSteps)]
    public static void AddSteps(Message message)
    {
        PlayerCharacter.Singleton.AddSteps(message.GetInt());
    }
    #endregion
    [MessageHandler((ushort)ServerToClientId.sendMonthlySteps)]
    public static void AddMonthlySteps(Message message)
    {
        StepTracker.Singleton.SetTotalSteps(message.GetInt());
    }
    private void Awake()
    {
        Singleton = this;
    }

}
