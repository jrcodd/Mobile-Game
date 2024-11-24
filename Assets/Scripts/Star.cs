using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    [SerializeField] private Sprite filledImage;
    [SerializeField] private Sprite emptyImage;
    private bool isFilled;
    public bool IsFilled
    {
        get { return isFilled; }
        set
        {
            isFilled = value;
            UpdateImage();
        }
    }
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


