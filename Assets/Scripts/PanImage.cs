using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PanType
{
    Loop = 0,
    PingPong = 1,
}
public class PanImage : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private PanType panType = PanType.Loop;
    private Vector3 startPosition;
    private void OnEnable()
    {
        startPosition = transform.position;
    }
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
