using UnityEngine;

public class ChangeMenus : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject next;
    [SerializeField] private GameObject current;
    public void Click()
    {
        current.SetActive(false);
        next.SetActive(true);
    }
}
