using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateSteps : MonoBehaviour
{
public TextMeshProUGUI stepsText;
    public void UpdateStepsText()
    {
        stepsText.text = $"{PlayerCharacter.Singleton.GetSteps()}";
    }
    private void FixedUpdate()
    {
        UpdateStepsText();
    }
}
