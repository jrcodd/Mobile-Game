using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
 
public class Base : MonoBehaviour
{
    [SerializeField] private int mapSize = 10; // The size of the map so 5 means that things can spawn with (-5,-5) to (5,5)
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private GameObject upgradeCanvas;
    [SerializeField] private GameObject MarketCanvas;
    [SerializeField] private GameObject raceCanvas;
    [SerializeField] private GameObject current;
    [SerializeField] private List<GameObject> regions;
    private Button button;
    [SerializeField] private float offset = 25f;

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
    private void OnEnable()

    {
        // Create button
        if (button == null)
        {
            button = Instantiate(buttonPrefab, canvas.transform);
        }
        button.gameObject.SetActive(false);

        GenerateMap();
    }
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
    private void GenerateMap()
    {
        foreach (GameObject region in regions   )
        {
            region.SetActive(true);
        }
    }

    public void ShowButton(Transform endObject)
    {
        // Position button in the blow the regon of the line
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

    public void HideButton()
    {
        if (button != null)
        {
            Invoke(nameof(DisableLineAndButton), 0.1f);
        }
    }
    void DisableLineAndButton()
    {
        if (button.gameObject.activeSelf)
        {
            button.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        Singleton = this;
    }
}
