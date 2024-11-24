using Riptide;
using UnityEngine;


[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Animator))]

public class PlayerMovementMultiplayer : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Health healthObject;

    private (Vector2, bool, float, float) inputs; //joystick direction, dodge, equipped item damage, health
    private void OnEnable()
    {
        inputs = new();
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FixedJoystick>();
        healthObject = GetComponent<Health>();

    }
    private void Update()
    {
        //joystick direction
        Vector2 moveDirection = new Vector3(joystick.Horizontal, joystick.Vertical).normalized;
        
        //dodge bool
        bool dodge = false;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended && touch.deltaTime < 0.2f)
            {
                dodge = true;
            }
        }

        //equpped item damage
        float damage = PlayerCharacter.Singleton.equippedItem.damage == 0 ? 1 : PlayerCharacter.Singleton.equippedItem.damage;

        //health
        float health = healthObject.GetHealth();
        
        inputs = (moveDirection, dodge, damage, health);
    }
    public bool GetInputGreaterThanZero()
    {
        return inputs.Item1.magnitude > 0;
    }
    private void FixedUpdate()
    {
        SendInput();
    }
    private void SendInput()
    {
        MessageSendMode sendMode = MessageSendMode.Unreliable;

        if (inputs.Item2) sendMode = MessageSendMode.Reliable;
        //joystick direction, dodge, equipped item damage, health
        Message message = Message.Create(sendMode, ClientToServerId.input);
        message.AddVector2(inputs.Item1);
        message.AddBool(inputs.Item2);
        message.AddFloat(inputs.Item3);
        message.AddFloat(inputs.Item4);
        NetworkManager.Singleton.Client.Send(message);

    }
}
