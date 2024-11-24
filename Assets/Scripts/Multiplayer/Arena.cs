using Riptide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
enum ArenaType
{
    Cave = 0,
    Boss,
}
public class Arena : MonoBehaviour
{
    //static is only used because riptide made me do it
    //(ig you can use messageRecieved instead but that requires
    //a rewrite of all the message handlers)
    [SerializeField] private ArenaType arenaType = ArenaType.Cave;
    [SerializeField] private GameObject[] enemyPrefabs;
    private Dictionary<ushort, EnemyControllerMultiplayer> enemyConrollerList;

    private static Arena _singleton;
    public static Arena Singleton
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
                Debug.Log($"{nameof(Arena)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }
    private void OnEnable()
    {
        ActivateArena();
    }
    private void Spawn(ushort id, EnemyType type, Vector3 position)
    {
        enemyConrollerList ??= new();
        GameObject _newEnemy = Instantiate(enemyPrefabs[(int) type], position, transform.rotation, transform);
        _newEnemy.GetComponent<EnemyControllerMultiplayer>().Arena = this;
        enemyConrollerList.Add(id, _newEnemy.GetComponent<EnemyControllerMultiplayer>());
    }

    private void ActivateArena()
    {
        enemyConrollerList = new();
        Message message = Message.Create(MessageSendMode.Reliable, (ushort) ClientToServerId.activateArena);
        message.Add((ushort)arenaType);
        NetworkManager.Singleton.Client.Send(message);
    }
    private void RemoveEnemy(ushort id)
    {
        if(enemyConrollerList.TryGetValue(id, out EnemyControllerMultiplayer enemyController))
        {
            Destroy(enemyController.gameObject);
            enemyConrollerList.Remove(id);
        }

    }
    private void Purge()
    {
        foreach (EnemyControllerMultiplayer enemy in enemyConrollerList.Values)
        {
            Destroy(enemy.gameObject);
        }
    }
    int enemyListLength;
    #region Messages
    [MessageHandler((ushort)ServerToClientId.enemyData)]
    private static void EnemyData(Message message)
    {
       
        if(Singleton == null)
        {
            return;
        }
        ushort id = message.GetUShort();
        int enemyType = message.GetInt();
        Vector3 position = message.GetVector3();

        if (Arena.Singleton.enemyConrollerList.TryGetValue(id, out EnemyControllerMultiplayer enemy))
        {
            enemy.SetInputs(position, message.GetVector3());
            enemy.GetHealthComponent().SetHealth(message.GetFloat());
            enemy.GetHealthComponent().SetMaxHealth(message.GetFloat());
            enemy.SetIndicatorFillAmount(message.GetFloat());
            enemy.SetAttackType(message.GetInt());
        }
        else
        {
            if (Arena.Singleton.enemyConrollerList.Count < Arena.Singleton.enemyListLength)
            {
                print(enemyType);
                if (enemyType < 3)
                {
                    if (Arena.Singleton != null && position != null)
                    {
                        Arena.Singleton.Spawn(id, (EnemyType)enemyType, position);
                    }
                }
            }
        }
    }
    [MessageHandler((ushort)ServerToClientId.enemyNum)]
    private static void EnemyNum(Message message)
    {
        if (Singleton == null)
        {
            return;
        }
        Arena.Singleton.enemyListLength = message.GetInt();
        if (Arena.Singleton.enemyListLength < Arena.Singleton.enemyConrollerList.Count)
        {
            Arena.Singleton.Purge();
            Arena.Singleton.enemyConrollerList.Clear();
        }
    }
[MessageHandler((ushort)ServerToClientId.enemyDied)]
    private static void EnemyDeath(Message message)
    {
        if (Singleton == null)
        {
            return;
        }
        Arena.Singleton.RemoveEnemy(message.GetUShort());
        
    }

    #endregion
    private void Awake()
    {
        Singleton = this;
    }
}
