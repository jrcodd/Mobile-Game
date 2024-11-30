
using UnityEngine;

///<summary>
/// Enum for the type of region
///</summary>
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

///<summary>
/// This script is a general script for all of the regions on the map. 
///</summary>
///<author>Jackson Codd</author>
///<version>1.0 Build 2024.11.29</version>
public class Region : MonoBehaviour
{
    ///<summary>
    /// The type of region that this is
    ///</summary>
    public RegionType regionType;

    ///<summary>
    /// The renderers of the region so that I can highlight them
    ///</summary>
    [SerializeField] private Renderer[] rends;

    ///<summary>
    /// The original color of the region so I can unhighlight it
    ///</summary>
    private Color originalColor;

    ///<summary>
    /// The canvas that will be shown when the region is clicked
    ///</summary>
    private GameObject regionCanvas;
   
    ///<summary>
    /// Instantiate the original color of the region
    void Start()
    {
        foreach (Renderer rend in rends)
        {
            if(rend != null && rend.material != null && rend.material.color != null)
            {
                originalColor = rend.material.color;
            }
        }
    }

    ///<summary>
    /// Highlight the region after a delay
    ///</summary>
    ///<param name="delay">The amount of time to wait before highlighting</param>
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


