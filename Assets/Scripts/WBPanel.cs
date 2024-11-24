using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Riptide;

public class WBPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI welcomeBackText;
    [SerializeField] private TextMeshProUGUI stepsText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private Slider slider;
    private int stepsFromTracker;

    private void OnEnable()
    {
        
        stepsFromTracker = StepTracker.Singleton.Steps;
        //for testing
        stepsFromTracker = 111500;
        welcomeBackText.text = $"Welcome Back! You Earned {stepsFromTracker} Steps While You Were Away!";
        stepsText.text = $"{stepsFromTracker}";
        xpText.text = $"{0}";
    }
    private void Update()
    {
        if(stepsFromTracker == 0)
        {
            Invoke(nameof(GetStepsAgain), 4);
        }
    }
    private void GetStepsAgain()
    {
        StepTracker.Singleton.GetSteps();
        stepsFromTracker = StepTracker.Singleton.Steps;

    }

    private void FixedUpdate()
    {
        float value = slider.value;
        int steps = (int) (stepsFromTracker * (1- value));
        int xp = stepsFromTracker - steps;
        stepsText.text = $"{steps}";
        xpText.text = $"{xp}";
    }


}
