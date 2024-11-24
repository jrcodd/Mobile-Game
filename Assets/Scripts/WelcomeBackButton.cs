using TMPro;
using UnityEngine;
using Riptide;

public class WelcomeBackButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI stepsText;
    [SerializeField] private GameObject map;


    public void Click()
    {
        int xp = 0;
        int steps = 0;
        try
        {
            xp = int.Parse(xpText.text);
        }
        catch
        {
            xp = 0;
            Debug.LogError("Failed to parse xp text");
        }
        try
        {
            steps = int.Parse(stepsText.text);
        }
        catch
        {
            Debug.LogError("Failed to parse steps text");
            steps = 0;
        }
        PlayerCharacter.Singleton.AddXp(xp);
        PlayerCharacter.Singleton.AddSteps(steps);
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.saveMonthlySteps);
        message.AddString(SystemInfo.deviceUniqueIdentifier);
        StepTracker.Singleton.totalSteps += steps + xp;
        message.AddInt(StepTracker.Singleton.totalSteps);
        NetworkManager.Singleton.Client.Send(message);
        map.SetActive(true);
    }
}
