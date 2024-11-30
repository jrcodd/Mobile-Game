using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Riptide;

///<summary>
///This script is responsible for updating the welcome back panel with the steps the player has earned while they were away
///</summary>
///<author>Jackson Codd</author>
///<version>1.0 Build 2024.11.29</version>
public class WBPanel : MonoBehaviour
{
    [Header("UI")]
    /// <summary>
    /// The text object that will show the welcome back message
    /// </summary>
    [SerializeField] private TextMeshProUGUI welcomeBackText;

    /// <summary>
    /// The text object that will show the steps the player has earned
    /// </summary>
    [SerializeField] private TextMeshProUGUI stepsText;

    /// <summary>
    /// The text object that will show the xp the player has earned
    /// </summary>
    [SerializeField] private TextMeshProUGUI xpText;

    /// <summary>
    /// The slider that allows the player to choose how many of the steps are converted to xp
    /// </summary>
    [SerializeField] private Slider slider;

    /// <summary>
    /// The steps the player has earned while they were away
    /// </summary>
    private int stepsFromTracker;

    /// <summary>
    // get the steps from the tracker and update the welcome back panel
    /// </summary>
    private void OnEnable()
    {
        
        stepsFromTracker = StepTracker.Singleton.Steps;
        //for testing I am setting the steps to a super high amount so I can actually play the game delete this line for testing. It works as of the build date on this class
        stepsFromTracker = 10000;
        welcomeBackText.text = $"Welcome Back! You Earned {stepsFromTracker} Steps While You Were Away!";
        stepsText.text = $"{stepsFromTracker}";
        xpText.text = $"{0}";
    }

    /// <summary>
    /// try and get the steps from the tracker until it gets them
    /// </summary>
    private void Update()
    {
        if(stepsFromTracker == 0)
        {
            Invoke(nameof(GetStepsAgain), 4);
        }
    }

    /// <summary>
    /// get the steps from the tracker
    /// </summary>
    private void GetStepsAgain()
    {
        StepTracker.Singleton.GetSteps();
        stepsFromTracker = StepTracker.Singleton.Steps;

    }

    /// <summary>
    /// update the steps and xp text with the new values
    /// </summary>
    private void FixedUpdate()
    {
        float value = slider.value;
        int steps = (int) (stepsFromTracker * (1- value));
        int xp = stepsFromTracker - steps;
        stepsText.text = $"{steps}";
        xpText.text = $"{xp}";
    }


}
