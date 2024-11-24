using UnityEngine;

public class GameWorldButton : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject rpg;

    public void Click()
    {
        
        //transfer items and eveything to rpg world
        mainMenu.SetActive(false);
        rpg.SetActive(true); 
        
    }
}
