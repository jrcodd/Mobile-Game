using UnityEngine;

public class AttackIndicatorController : MonoBehaviour
{
    public Transform fillIndicator;
    private float fillAmount;

    public void SetFillAmount(float amount)
    {
        fillAmount = amount;
        fillIndicator.localScale = new Vector3(fillAmount, 1, 1);
    }
    public float GetFillAmount()
    {
        return fillAmount;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}