
using UnityEngine;

public enum RegionType
{
    empty = 0,
    resource = 1,
    cave = 2,
    boss = 3,
    home = 4,
    market = 5,
    race = 6,
    upgrades = 7

}
public class Region : MonoBehaviour
{
    public RegionType regionType;

    [SerializeField] private Renderer[] rends;
    private Color originalColor;
    private GameObject regionCanvas;
   

    void Start()
    {
        foreach (Renderer rend in rends)
        {
            //originalColor = rend.material.color;
        }
    }
    public void DelayedHighlight(float delay)
    {
        Invoke(nameof(Highlight), delay);
    }
    public void Highlight()
    {
        if ((int)regionType < 5)
        {
            Map.Singleton.ShowDashedLineAndButton(transform);
        }
        else
        {
            Base.Singleton.ShowButton(transform);
        }

        foreach (Renderer rend in rends)
        {
            //rend.material.color = Color.yellow; // Or any color you prefer
            //Invoke("ResetColor", 0.5f); // Reset color after 0.5 seconds
        }
        
    }

    private void ResetColor()
    {
        foreach (Renderer rend in rends)
        {
            rend.material.color = originalColor;
        }
    }
}


