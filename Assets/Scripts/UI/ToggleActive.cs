using UnityEngine;

public class ToggleActive : MonoBehaviour
{
    [SerializeField] private GameObject target;

    //toggle the target object active state
    public void Toggle()
    {
        target.SetActive(!target.activeSelf);
    }
}
