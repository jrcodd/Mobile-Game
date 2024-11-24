using UnityEngine;
using Riptide;
using Riptide.Utils;
using System;
public enum ServerToClientId : ushort
{
    sync = 1,
    playerSpawned,
    item,
    itemListed,
    clearMarketList,
    buyItem,
    itemSold,
    sendInventory,
    sendCoins,
    sendXp,
    sendSteps,
    playerMovement,
    gameOver,
    enemyData,
    playerHealth,
    enemyDied,
    enemyNum,
    sendMonthlySteps,

}
public enum ClientToServerId : ushort
{
    name = 1,
    item,
    listItem,
    clearMarketList,
    buyItem,
    itemSold,
    saveInventory,
    saveCoins,
    saveXp,
    getPlayerData,
    saveSteps,
    input,
    spawnPos,
    activateArena,
    saveMonthlySteps,
}

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    public static NetworkManager Singleton
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
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }
    public Riptide.Client Client { get; private set; }

    private ushort _serverTick;
    public ushort ServerTick
    {
        get => _serverTick;
        private set
        {
            _serverTick = value;
            InterpolationTick = (ushort)(value - TicksBetweenPositionUpdates);
        }
    }
    public ushort InterpolationTick { get; private set; }
    private ushort _ticksBetweenPositionUpdates = 2;
    public ushort TicksBetweenPositionUpdates
    {
        get => _ticksBetweenPositionUpdates;
        private set
        {
            _ticksBetweenPositionUpdates = value;
            InterpolationTick = (ushort)(ServerTick - value);
        }
    }
    [SerializeField] private string ip;
    [SerializeField] private ushort port;
    [Space(10)]
    [SerializeField] private ushort tickDivergenceTolerance = 1;

    private void Awake()
    {
        Singleton = this;
    }
    private void Start()
    {

        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        Client = new Riptide.Client();
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.Disconnected += DidDisconnect;
        Client.ClientDisconnected += PlayerLeft;
        ServerTick = 2;
        
    }
    public void FixedUpdate()
    {
        Client.Update();
        ServerTick++;
    }
    public void OnApplicationQuit()
    {
        Client.Disconnect();
    }
    public void Connect()
    {
        Client.Connect($"{ip}:{port}");
        PlayerCharacter.Singleton.GetPlayerData();

        

    }
    private void DidConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.ConnectionSuccess();
    }
    private void FailedToConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
    }
    private void PlayerLeft(object seder, ClientDisconnectedEventArgs e)
    {
        if(Player.list.TryGetValue(e.Id, out Player player))
        {
            Destroy(Player.list[e.Id].gameObject);
        }
    }
    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
        foreach (Player player in Player.list.Values)
        {
            Destroy(player.gameObject);
        }

    }
    private void SetTick(ushort serverTick)
    {
        if (Mathf.Abs(ServerTick - serverTick) > tickDivergenceTolerance)
        {
            print($"Client tick: {ServerTick} -> {serverTick}");
            ServerTick = serverTick;
        }
    }
    [MessageHandler((ushort) ServerToClientId.sync)]
    public static void Sync(Message message)
    {
        Singleton.SetTick(message.GetUShort());
    }

}
