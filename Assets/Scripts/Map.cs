using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

///<summary>
///This script is for the map that the player can interact with
///</summary>
///<author>Jackson Codd</author>
///<version>1.0 Build 2024.11.24</version>
public class Map : MonoBehaviour
{
    [Header("Map Settings")]
    /// <summary>
    /// The size of the map so 5 means that things can spawn with (-5,-5) to (5,5)
    /// </summary>
    [SerializeField] private int mapSize = 10; 

    /// <summary>
    /// The number of regions to spawn in the map
    /// </summary>
    [SerializeField] private int numSpawns = 10; 
    
    /// <summary>
    /// The object that the dashed line will start at
    /// </summary>
    [SerializeField] private Transform startObject;

    /// <summary>
    /// The length of the dash
    /// </summary>
    [SerializeField] private float dashLength = 0.2f;

    /// <summary>
    /// The length of the gap between dashes
    /// </summary>
    [SerializeField] private float gapLength = 0.1f;

    /// <summary>
    /// The width of the line
    /// </summary>
    [SerializeField] private float lineWidth = 0.1f;
    [Header("Object References")]
    /// <summary>
    /// The prefabs of the regions that can spawn
    /// </summary>
    [SerializeField] private GameObject[] regionPrefabs;

    /// <summary>
    /// The material of the line
    /// </summary>
    [SerializeField] private Material lineMaterial;

    /// <summary>
    /// The canvas that the button will be placed on
    /// </summary>
    [SerializeField] private Canvas canvas;

    /// <summary>
    /// The button prefab that will be instantiated
    /// </summary>
    [SerializeField] private Button buttonPrefab;

    /// <summary>
    /// The canvas that will be shown when the cave button is clicked
    /// </summary>
    [SerializeField] private GameObject caveCanvas;

    /// <summary>
    /// The canvas that will be shown when the boss button is clicked
    /// </summary>
    [SerializeField] private GameObject bossCanvas;

    /// <summary>
    /// The canvas that will be shown when the base button is clicked
    /// </summary>
    [SerializeField] private GameObject baseCanvas;

    /// <summary>
    /// The canvas that will be shown when the resource button is clicked
    /// </summary>
    [SerializeField] private GameObject resourceCanvas;

    /// <summary>
    /// The current location that is highlighted
    /// </summary>
    [SerializeField] private GameObject current;

    /// <summary>
    /// The list of regions that are on the map
    /// </summary>
    private List<GameObject> regions;

    /// <summary>
    /// The line renderer that will be used to draw the dashed line
    /// </summary>
    private LineRenderer lineRenderer;

    /// <summary>
    /// The button that will be shown
    /// </summary>
    private Button button;

    /// <summary>
    /// The singleton instance of the map
    /// </summary>
    private static Map _singleton;
    public static Map Singleton
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
                print($"{nameof(Map)} instance already exists, destroying duplicate!");
                Destroy(value);
            }

        }
    }

    /// <summary>
    /// When the object is enabled, create the line renderer and button
    /// </summary>
    private void OnEnable()
    {
        if (gameObject.GetComponent<LineRenderer>() == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
        }
        lineRenderer.enabled = false;
        if (button == null)
        {
            button = Instantiate(buttonPrefab, canvas.transform);
        }
        button.gameObject.SetActive(false);

        if (regions == null)
        {
            regions = new();

        }
        if (regions.Count > 0)
        {
            foreach (GameObject obj in regions)
            {
                obj.SetActive(true);
            }
        }
        GenerateMap();
    }

    /// <summary>
    /// When the object is disabled disable all of the regions
    /// </summary>
    private void OnDisable()
    {
        if(regions != null && regions.Count > 0) {
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
    /// Generate the map by spawning the regions
    /// </summary>
    private void GenerateMap()
    {
        if (regions.Count == 0)
        {
            for (int i = 0; i < numSpawns; i++)
            {
                GameObject region = regionPrefabs[Random.Range(0, regionPrefabs.Length)];
                Vector3 spawnPos = new Vector3(Random.Range(-mapSize, mapSize), 0, Random.Range(-mapSize, mapSize));
                GameObject regionObject = Instantiate(region, spawnPos, region.transform.rotation);
                regions.Add(regionObject);
            }
        }
    }

    /// <summary>
    /// Show the dashed line and button
    /// </summary>
    /// <param name="endObject">The object that the dashed line will end at</param>
    public void ShowDashedLineAndButton(Transform endObject)
    {
        if (startObject == null || endObject == null)
        {
            Debug.LogError("Start or end object is not assigned!");
            return;
        }

        Vector3 startPosition = startObject.position;
        Vector3 endPosition = endObject.position;

        // Calculate dash count
        float lineLength = Vector3.Distance(startPosition, endPosition);
        int dashCount = Mathf.FloorToInt(lineLength / (dashLength + gapLength));

        // Set up line renderer
        lineRenderer.positionCount = dashCount * 2;
        lineRenderer.enabled = true;

        // Create dashed line
        for (int i = 0; i < dashCount; i++)
        {
            float startT = i * (dashLength + gapLength) / lineLength;
            float endT = startT + dashLength / lineLength;

            lineRenderer.SetPosition(i * 2, Vector3.Lerp(startPosition, endPosition, startT));
            lineRenderer.SetPosition(i * 2 + 1, Vector3.Lerp(startPosition, endPosition, endT));
        }

        // Position button in the middle of the line
        Vector3 buttonPosition = Vector3.Lerp(startPosition, endPosition, 0.5f);
        button.transform.position = Camera.main.WorldToScreenPoint(buttonPosition);
        Region selectedRegion = endObject.gameObject.GetComponent<Region>();
        if(selectedRegion.regionType == RegionType.cave)
        {
            button.gameObject.GetComponent<TravelButton>().UpdateButton(caveCanvas, current, (int) lineLength*100);
        } else if(selectedRegion.regionType == RegionType.boss)
        {
            button.gameObject.GetComponent<TravelButton>().UpdateButton(bossCanvas, current, (int) lineLength*100);
        }
        else if(selectedRegion.regionType == RegionType.home)
        {
            button.gameObject.GetComponent<TravelButton>().UpdateButton(baseCanvas, current, (int) lineLength*100);
        }
        else if(selectedRegion.regionType == RegionType.resource)
        {
            button.gameObject.GetComponent<TravelButton>().UpdateButton(resourceCanvas, current, (int) lineLength*100);
        }
        button.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide the dashed line and button
    /// </summary>
    public void HideDashedLineAndButton()
    {
        if (lineRenderer != null)
        {
            Invoke(nameof(DisableLineAndButton), 0.1f);
        }
    }

    /// <summary>
    /// Disable the line and button
    /// </summary>
    void DisableLineAndButton()
    {
        lineRenderer.enabled = false;
        if (button.gameObject.activeSelf)
        {
            button.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// When the object is destroyed, destroy the button
    /// </summary>
    private void Awake()
    {
        Singleton = this;
        gameObject.SetActive(false);
    }
}
