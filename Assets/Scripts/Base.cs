using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
 
/// <summary>
/// This script is for the base location on the map
/// </summary>
/// <author>Jackson Codd</author>
/// <version>1.0 Build 2024.11.24</version>
public class Base : MonoBehaviour
{

    /// <summary>
    /// The size of the map so 5 means that things can spawn with (-5,-5) to (5,5)
    /// </summary>
    [SerializeField] private int mapSize = 10; 

    /// <summary>
    /// The canvas that the button will be placed on
    /// </summary>
    [SerializeField] private Canvas canvas;

    /// <summary>
    /// The button prefab that will be instantiated
    /// </summary>
    [SerializeField] private Button buttonPrefab;

    /// <summary>
    /// The canvas that will be shown when the button is clicked
    /// </summary>
    [SerializeField] private GameObject upgradeCanvas;

    /// <summary>
    /// The canvas that will be shown when the button is clicked
    /// </summary>
    [SerializeField] private GameObject MarketCanvas;

    /// <summary>
    /// The canvas that will be shown when the button is clicked
    /// </summary>
    [SerializeField] private GameObject raceCanvas;

    /// <summary>
    /// The current location that is highlighted
    /// </summary>
    [SerializeField] private GameObject current;

    /// <summary>
    /// The list of regions that are on the map
    /// </summary>
    [SerializeField] private List<GameObject> regions;

    /// <summary>
    /// The button that will be shown
    /// </summary>
    private Button button;

    /// <summary>
    /// The offset of the button
    /// </summary>
    [SerializeField] private float offset = 25f;

    /// <summary>
    /// The singleton instance of the base
    /// </summary>
    private static Base _singleton;
    public static Base Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                print($"{nameof(Base)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }

    /// <summary>
    /// This code is run when the script is enabled. So the button and map are instantiated.
    /// </summary>
    private void OnEnable()
    {
        if (button == null)
        {
            button = Instantiate(buttonPrefab, canvas.transform);
        }
        button.gameObject.SetActive(false);

        GenerateMap();
    }
    
    /// <summary>
    /// This code is run when the script is disabled. So the map is hidden.
    /// </summary>
    private void OnDisable()
    {
        if (regions != null && regions.Count > 0)
        {
            foreach (GameObject obj in regions)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// activates all of the regionss
    /// </summary>
    private void GenerateMap()
    {
        foreach (GameObject region in regions)
        {
            region.SetActive(true);
        }
    }

    /// <summary>
    /// This method creates a line from the region that the player is in to the region where they want to go
    /// </summary>
    public void ShowButton(Transform endObject)
    {
        Vector3 buttonPosition = Vector3.Lerp(endObject.position, endObject.position - new Vector3(0,0, offset), 0.5f);
        button.transform.position = Camera.main.WorldToScreenPoint(buttonPosition);
        Region selectedRegion = endObject.gameObject.GetComponent<Region>();
        if (selectedRegion.regionType == RegionType.market)
        {
            button.gameObject.GetComponent<BaseButton>().UpdateButton(MarketCanvas, current, ResourceType.Stone, 5);
        }
        else if (selectedRegion.regionType == RegionType.race)
        {
            button.gameObject.GetComponent<BaseButton>().UpdateButton(raceCanvas, current, ResourceType.Stone, 5);
        }
        else if (selectedRegion.regionType == RegionType.upgrades)
        {
            button.gameObject.GetComponent<BaseButton>().UpdateButton(upgradeCanvas, current, ResourceType.Stone, 5);
        }
       
        button.gameObject.SetActive(true);
    }

    /// <summary>
    /// This method hides the button
    /// </summary>
    public void HideButton()
    {
        if (button != null)
        {
            Invoke(nameof(DisableLineAndButton), 0.1f);
        }
    }

    /// <summary>
    /// This method disables the line and button
    /// </summary>
    void DisableLineAndButton()
    {
        if (button.gameObject.activeSelf)
        {
            button.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Instantiate the singleton object when the script is awake
    /// </summary>
    private void Awake()
    {
        Singleton = this;
    }
}
