using UnityEngine;
public class BackToMainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject inventory;
    public void Click()
    {
        print("e");
        inventory.SetActive(false);
        mainMenu.SetActive(true);

    }
}
