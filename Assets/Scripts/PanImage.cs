using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Enum for the type of panning
///</summary>
enum PanType
{
    Loop = 0,
    PingPong = 1,
}

///<summary>
/// This script is for the motion of th ebackground images in the menu ui's
///</summary>
///<author>Jackson Codd</author>
///<version>1.0 Build 2024.11.24</version>
public class PanImage : MonoBehaviour
{
    ///<summary>
    /// The speed at which the image will pan
    ///</summary>
    [SerializeField] private float speed = 0.5f;

    ///<summary>
    /// The type of panning that will be used
    ///</summary>
    [SerializeField] private PanType panType = PanType.Loop;

    ///<summary>
    /// The start position of the image
    ///</summary>
    private Vector3 startPosition;

    ///<summary>
    /// Initialize the start position of the image
    ///</summary>
    private void OnEnable()
    {
        startPosition = transform.position;
    }

    ///<summary>
    /// Update the position of the image depending on which type of panning is selected
    ///</summary>
    private void FixedUpdate()
    {
        switch (panType)
        {
            case PanType.Loop:
                loopImage();
                break;
            case PanType.PingPong:
                pingPongImage();
                break;
        }
    }

    /// <summary>
    /// This function will make the image background loop over itself
    /// </summary>
    private void loopImage()
    {
        float imageWidth = transform.GetComponent<RectTransform>().rect.width;
        float screenWidth = GetComponentInParent<Canvas>().pixelRect.width;
        float x = transform.position.x;
        float y = transform.position.y;
        float newX = x + Mathf.Sin(Time.deltaTime * speed) * screenWidth;
        transform.position = new (newX, y, 0);
        if (x < - ((imageWidth / 2) - (screenWidth/2)))
        {
            transform.position = new Vector3(((imageWidth / 2) - (screenWidth / 2)), y, 0);
        }
        if (x > ((imageWidth / 2) - (screenWidth / 2)))
        {
            transform.position = new Vector3(-((imageWidth / 2) - (screenWidth / 2)), y, 0);
        }
    }

    /// <summary>
    /// This function will make the image background bounce off the edges
    /// </summary>
    private void pingPongImage()
    {
        float imageWidth = transform.GetComponent<RectTransform>().rect.width;
        float screenWidth = transform.parent.GetComponent<RectTransform>().rect.width;
        float x = transform.localPosition.x;
        float y = transform.localPosition.y;
        float time = Time.deltaTime;
        float newX = x + Mathf.Sin(time * speed) * screenWidth;
        transform.localPosition = new (newX, y, 0);
        if (x < -((imageWidth / 2) - (screenWidth / 2)))
        {
            transform.localPosition = new Vector3(1 -((imageWidth / 2) - (screenWidth / 2)), y, 0);
            speed *= -1;
        }
        if (x > ((imageWidth / 2) - (screenWidth / 2)))
        {
            transform.localPosition = new Vector3(-1 + ((imageWidth / 2) - (screenWidth / 2)), y, 0);

            speed *= -1;
        }
    }
}
