using Riptide;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
    public ushort Id { get; private set; }

    public bool IsLocal { get; private set; }
    private string username;

    [SerializeField] private Interpolator interpolator;
    [SerializeField] private PlayerAnimationManager animationManager;
    private void OnDestroy()
    {
        list.Remove(Id);
    }
    private void Move(ushort tick, bool isDodging, Vector3 newPosition, Vector3 forward)
    {
        if (!isDodging)
        {
            interpolator.NewUpdate(tick, newPosition);
        }
        transform.forward = forward;
        animationManager.AnimateBasedOnInput();
        animationManager.AnimateAttack();
    }
    public static GameObject GetLocalPlayer()
    {
        if (list.TryGetValue(NetworkManager.Singleton.Client.Id, out Player player))
        {

            return player.gameObject;
        }
        else
        {
            return GameObject.FindGameObjectWithTag("Player");
        }
    }
    private static Vector3 spawnPos;
    public static void SetSpawnPos(Vector3 pos)
    {
        spawnPos = pos;
        Message message = Message.Create(MessageSendMode.Reliable, ClientToServerId.spawnPos);
        message.AddVector3(pos);
        NetworkManager.Singleton.Client.Send(message);
    }
    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if(id == NetworkManager.Singleton.Client.Id)
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, spawnPos, Quaternion.identity).GetComponent<Player>();
            print("spawned local player");
            player.IsLocal = true;
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, spawnPos, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }
        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.Id = id;
        player.username = username;

        list.Add(id, player);
    }
    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }
    [MessageHandler((ushort)ServerToClientId.playerMovement)]
    private static void HandlePlayerMovement(Message message)
    {
        if(list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.Move(message.GetUShort(), message.GetBool(), message.GetVector3(), message.GetVector3());
            bool isAttacking = message.GetBool();
            player.GetComponent<PlayerAnimationManager>().SetAttacking(isAttacking);
        }
    }
    [MessageHandler((ushort)ServerToClientId.playerHealth)]
    public static void SetHealth(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.GetComponent<Health>().SetHealth(message.GetFloat());
        }
    }
}

