using TMPro;
using UnityEngine;
using Riptide;

/// <summary>
/// This script is for the welcome back button that will add the steps and xp to the player to confirm the selection
/// </summary>
/// <author>Jackson Codd</author>
/// <version>1.0 Build 2024.11.29</version>
public class WelcomeBackButton : MonoBehaviour
{
    /// <summary>
    /// The text object that will show the xp that the player will get
    /// </summary>
    [SerializeField] private TextMeshProUGUI xpText;

    /// <summary>
    /// The text object that will show the steps that the player will get
    /// </summary>
    [SerializeField] private TextMeshProUGUI stepsText;

    /// <summary>
    /// The map that will be enabled when the button is clicked
    /// </summary>
    [SerializeField] private GameObject map;

    /// <summary>
    /// When the button is clicked, add the steps and xp to the player
    /// </summary>
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
        //send the step data to the server
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.saveMonthlySteps);
        message.AddString(SystemInfo.deviceUniqueIdentifier);
        StepTracker.Singleton.totalSteps += steps + xp;
        message.AddInt(StepTracker.Singleton.totalSteps);
        NetworkManager.Singleton.Client.Send(message);
        map.SetActive(true);
    }
}
