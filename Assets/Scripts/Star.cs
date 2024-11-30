using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The script for the difficulty stars on certain screens
/// </summary>
/// <author>Jackson Codd</author>
/// <version>1.0 Build 2024.11.29</version>
public class Star : MonoBehaviour
{
    /// <summary>
    /// The sprite of the filled star image
    /// </summary>
    [SerializeField] private Sprite filledImage;

    /// <summary>
    /// The sprite of the empty star image
    /// </summary>
    [SerializeField] private Sprite emptyImage;

    /// <summary>
    /// kind of unnessesary way to implement this byt basically update the image when the filled status changes
    /// </summary>
    public bool IsFilled
    {
        get { return isFilled; }
        set
        {
            isFilled = value;
            UpdateImage();
        }
    }
    /// <summary>
    /// Update the image to reflect the filled status
    /// </summary>
    private void UpdateImage()
    {
        if (isFilled)
        {
            GetComponent<Image>().sprite = filledImage;
        }
        else
        {
            GetComponent<Image>().sprite = emptyImage;
        }
    }
}


