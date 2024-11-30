using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

///<summary>
///This script is responsible for updating the total steps text on the ui
///</summary>
public class UpdateSteps : MonoBehaviour
{
    /// <summary>
    /// The text that will be updated
    /// </summary>
    public TextMeshProUGUI stepsText;
    
    /// <summary>
    /// Update the steps text with the current steps from the player class
    /// </summary>
    public void UpdateStepsText()
    {
        stepsText.text = $"{PlayerCharacter.Singleton.GetSteps()}";
    }

    /// <summary>
    /// Update the steps text every frame Might change this so its not making a call to getsteps every frame
    /// </summary>
    private void FixedUpdate()
    {
        UpdateStepsText();
    }
}
